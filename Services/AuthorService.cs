using BookManagementSystem.Data;
using BookManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Services
{
    public class AuthorService : IAuthorService
    {
        ApplicationDbContext _db;

        public AuthorService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool AuthorExists(int id)
        {
            return (_db.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<Author?> GetAuthorByIdAsync(int? id)
        {
            return await _db.Authors.Include(x => x.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> createAuthorAsync(AuthorCreateViewModel model)
        {
            List<Category> categories = new List<Category>();
            foreach (int index in model.CategoryIdList)
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == index);
                if (category != null)
                {
                    categories.Add(category);
                }
            }
            model.Author.Categories = categories;
            _db.Add(model.Author);
            return await _db.SaveChangesAsync();
        }

        public async Task<AuthorCreateViewModel> createViewModel(AuthorCreateViewModel? viewModel = null, int? id = null)
        {
            if (viewModel != null)
            {
                viewModel.Categories = await _db.Categories.ToListAsync();
                return viewModel;
            }
            else
            {
                AuthorCreateViewModel viewModelResult = new AuthorCreateViewModel();
                viewModelResult.Categories = await _db.Categories.ToListAsync();
                if (id != null)
                {
                    viewModelResult.Author = await _db.Authors.Include(x => x.Categories).FirstOrDefaultAsync(c => c.Id == id);
                    if (viewModelResult.Author.Categories != null)
                    {
                        foreach (var category in viewModelResult.Author.Categories)
                        {
                            viewModelResult.CategoryIdList.Add(category.Id);
                        }
                    }
                }
                return viewModelResult;
            }
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

        public async Task<bool> UpdateAuthor(int id, AuthorCreateViewModel viewModel)
        {
            Author? author = await GetAuthorByIdAsync(id);
            if (author != null)
            {
                author.Name = viewModel.Author.Name;
                author.Country = viewModel.Author.Country;
                author.Description = viewModel.Author.Description;
                author.Categories = await GetCategoriesFromIdList(viewModel.CategoryIdList);

                _db.Authors.Update(author);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
