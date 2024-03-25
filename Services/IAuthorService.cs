using BookManagementSystem.Models;

namespace BookManagementSystem.Services
{
    public interface IAuthorService
    {
        bool AuthorExists(int id);
        Task<int> createAuthorAsync(AuthorCreateViewModel model);
        Task<AuthorCreateViewModel> createViewModel(AuthorCreateViewModel? viewModel = null, int? id = null);
        Task<bool> UpdateAuthor(int id, AuthorCreateViewModel viewModel);
    }
}
