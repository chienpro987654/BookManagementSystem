using BookManagementSystem.Models;

namespace BookManagementSystem.Services
{
    public interface IHomeService
    {
        public Task<HomeViewModel> CreateHomeViewModelAsync();
        Task<HomeSearchViewModel> createHomeSearchViewModel(string query, List<string> selectedCategories);
        Task<HomeViewModel> createBookViewModelFromCategory(int CategoryId, string sortOrder);
        List<Book> getOrderBookList(List<Book> books, string sortOrder);
    }
}
