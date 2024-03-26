using BookManagementSystem.Data;
using BookManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Services
{
    public class HomeService : IHomeService
    {
        ApplicationDbContext _db;

        public HomeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<HomeViewModel> CreateHomeViewModelAsync()
        {
            HomeViewModel viewModel = new HomeViewModel
            {
                BookRecommend = await _db.Books.OrderByDescending(x => x.Rating).Take(5).ToListAsync(),
                BooksRecent = await _db.Books.OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync(),
                BookTopSale = await _db.Books.OrderByDescending(x => x.SoldQuantity).Take(10).ToListAsync(),
            };
            return viewModel;
        }
        public async Task<HomeSearchViewModel> createHomeSearchViewModel(string query, List<string> selectedCategories)
        {
            HomeSearchViewModel viewModel = new HomeSearchViewModel();
            //query = query ?? string.Empty;
            List<int> CategoryIds = new List<int>();
            foreach (var str in selectedCategories)
            {
                if (int.TryParse(str, out var categoryId))
                {
                    CategoryIds.Add(categoryId);
                }
            }
            viewModel.sQuery = query;
            viewModel.Categories = await _db.Categories.ToListAsync();

            viewModel.BookResults = await _db.Books
                .Where(p => (selectedCategories.Count == 0 || p.Categories.Any(l => CategoryIds.Contains(l.Id))) && (query == null || p.Title.Contains(query) || p.Description.Contains(query)))
                .Include(x => x.Categories).Include(x => x.Author).ToListAsync();
            return viewModel;
        }
        public async Task<HomeViewModel> createBookViewModelFromCategory(int CategoryId, string sortOrder)
        {
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.Categories = await _db.Categories.ToListAsync();
            Category? category = await _db.Categories.Include(x => x.Books).Where(x => x.Id == CategoryId).FirstOrDefaultAsync();
            if (category != null && category.Books != null)
            {
                viewModel.BooksFromCategory = getOrderBookList(category.Books, sortOrder);
            }
            viewModel.CategoryIdSelected = CategoryId;
            viewModel.SortOrderSelected = sortOrder;
            return viewModel;
        }

        public List<Book> getOrderBookList(List<Book> books, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(x => x.Title).ToList();
                    break;
                case "name_asc":
                    books = books.OrderBy(x => x.Title).ToList();
                    break;
                case "upload_desc":
                    books = books.OrderByDescending(x => x.CreatedAt).ToList();
                    break;
                case "upload_asc":
                    books = books.OrderBy(x => x.Title).ToList();
                    break;
                case "publish_desc":
                    books = books.OrderByDescending(x => x.PublishDate).ToList();
                    break;
                case "publish_asc":
                    books = books.OrderBy(x => x.PublishDate).ToList();
                    break;
                default:
                    break;
            }
            return books;
        }
    }
}
