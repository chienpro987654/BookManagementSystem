using BookManagementSystem.Data;
using BookManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _db;
        private readonly string imageDirectoryPath;
        private readonly string fileDirectoryPath;

        public BookService(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            imageDirectoryPath = configuration["AppSettings:ImageDir"];
            fileDirectoryPath = configuration["AppSettings:FileDir"];

        }

        public bool IsValidImage(BookCreateViewModel model)
        {
            if (model.Image.Length > 0 && model.Image.ContentType.Contains("image"))
            {
                return true;
            }
            return false;
        }

        public bool isValidFile(BookCreateViewModel viewModel)
        {
            if (viewModel.File.Length > 0 && viewModel.File.ContentType.Contains("text"))
            {
                return true;
            }
            return false;
        }

        public async Task<string> UploadFile(IFormFile file, string oldFileName = "", string type = "image")
        {
            string directoryPath = type.Equals("image") ? imageDirectoryPath : fileDirectoryPath;
            string sResult = "";
            if (!oldFileName.Equals(string.Empty) && File.Exists(Path.Combine(directoryPath, oldFileName)))
            {
                File.Delete(Path.Combine(directoryPath, oldFileName));
            }
            string sExtension = Path.GetExtension(file.FileName);
            string sName = Path.GetRandomFileName().Replace(".", string.Empty);
            sName += sExtension;
            Directory.CreateDirectory(directoryPath);
            string filePath = Path.Combine(directoryPath, sName);
            using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);
            sResult = sName;
            return sResult;
        }

        public async Task<List<Category>> GetCategoriesFromIdList(List<int> idList)
        {
            List<Category> categories = new List<Category>();
            foreach (int index in idList)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == index);
                if (category != null)
                {
                    categories.Add(category);
                }
            }
            return categories;
        }

        public async Task<int> CreateBookAsync(BookCreateViewModel viewModel)
        {
            viewModel.Book.Categories = await GetCategoriesFromIdList(viewModel.CategoryIdList);
            _db.Add(viewModel.Book);

            if (viewModel.Image != null)
            {
                viewModel.Book.ImageName = await UploadFile(viewModel.Image);
            }
            if (viewModel.File != null)
            {
                viewModel.Book.FileName = await UploadFile(viewModel.File, "", "text");
            }

            var result = await _db.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _db.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int? id)
        {
            return await _db.Books.Include(x => x.Categories).Include(x => x.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAuthorList()
        {
            return await _db.Authors.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoryList()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<BookCreateViewModel> createViewModel(BookCreateViewModel? model = null, int? id = null)
        {

            if (model != null)
            {
                model.Categories = await _db.Categories.ToListAsync();
                model.Authors = await _db.Authors.ToListAsync();
                return model;
            }
            else
            {

                BookCreateViewModel result = new()
                {
                    Categories = await _db.Categories.ToListAsync(),
                    Authors = await _db.Authors.ToListAsync(),
                };
                if (id != null)
                {
                    Book _book = await GetBookByIdAsync(id);
                    result.Book = _book;

                    if (_book.Categories != null)
                    {
                        foreach (var category in _book.Categories)
                        {
                            result.CategoryIdList.Add(category.Id);
                        }
                    }
                }
                return result;
            }
        }
        public async Task<bool> DeleteBook(int id)
        {
            var book = await GetBookByIdAsync(id);
            if (book != null)
            {
                _db.Books.Remove(book);
                if (await _db.SaveChangesAsync() > 0)
                {
                    File.Delete(imageDirectoryPath + "/" + book.ImageName);
                    File.Delete(fileDirectoryPath + "/" + book.FileName);
                }
                return true;
            }
            return false;
        }

        public bool BookExists(int id)
        {
            return (_db.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<bool> UpdateBook(int id, BookCreateViewModel viewModel)
        {
            Book? book = await GetBookByIdAsync(id);
            if (book != null)
            {
                if (viewModel.Image != null)
                {
                    viewModel.Book.ImageName = await UploadFile(viewModel.Image, viewModel.Book.ImageName);
                }
                if (viewModel.File != null)
                {
                    viewModel.Book.ImageName = await UploadFile(viewModel.File, viewModel.Book.FileName, "file");
                }
                book.Title = viewModel.Book.Title;
                book.Description = viewModel.Book.Description;
                book.AuthorId = viewModel.Book.AuthorId;
                book.Quantity = viewModel.Book.Quantity;
                book.Price = viewModel.Book.Price;
                book.SoldQuantity = viewModel.Book.SoldQuantity;
                book.Rating = viewModel.Book.Rating;
                book.PublishDate = viewModel.Book.PublishDate;
                book.UpdatedAt = DateTime.Now;
                book.Categories = await GetCategoriesFromIdList(viewModel.CategoryIdList);

                _db.Books.Update(book);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<BookReadViewModel> CreateReadViewModel(int id, int page = -1)
        {
            BookReadViewModel viewModel = new BookReadViewModel
            {
                Book = await GetBookByIdAsync(id),
            };
            if (viewModel.Book != null)
            {
                string[] lines = File.ReadAllLines(Path.Combine(fileDirectoryPath, viewModel.Book.FileName));
                int pageTotal = 0;
                int wordCount = 0;
                List<string> ArrContent = new List<string>();
                List<int> arrIndexStart = new List<int>();
                arrIndexStart.Add(0);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (arrIndexStart.Count == page + 1)
                    {
                        ArrContent.Add(lines[i] + "\n");
                    }
                    wordCount += lines[i].Split(" ").Length;
                    if (wordCount >= 3000)
                    {
                        wordCount = 0;
                        pageTotal++;
                        arrIndexStart.Add(i++);
                    }
                    if (arrIndexStart.Count == 67)
                    {
                        int x = 0;
                    }
                }
                if (page != -1 && page <= arrIndexStart.Count)
                {
                    viewModel.PrevPage = (page == 0) ? -1 : page - 1;
                    viewModel.NextPage = (page >= arrIndexStart.Count - 1) ? -1 : page + 1;
                    viewModel.ArrContent = ArrContent;
                }
                viewModel.PageTotal = pageTotal;
                viewModel.ArrIndexStart = arrIndexStart;
            }
            return viewModel;
        }


    }
}
