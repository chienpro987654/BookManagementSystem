using BookManagementSystem.Models;

namespace BookManagementSystem.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int? id);

        Task<int> CreateBookAsync(BookCreateViewModel model);
        Task<IEnumerable<Author>> GetAuthorList();
        Task<IEnumerable<Category>> GetCategoryList();
        bool IsValidImage(BookCreateViewModel model);
        bool isValidFile(BookCreateViewModel viewModel);
        Task<BookCreateViewModel> createViewModel(BookCreateViewModel? model = null, int? id = null);
        Task<bool> DeleteBook(int id);
        Task<bool> UpdateBook(int id, BookCreateViewModel viewModel);
        bool BookExists(int id);
        Task<BookReadViewModel> CreateReadViewModel(int id, int page = -1);
    }
}
