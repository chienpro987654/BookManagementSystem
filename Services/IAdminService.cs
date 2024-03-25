using BookManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace BookManagementSystem.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<UserViewModel>> GetAllUser();
        IEnumerable<Book> GetListBook();
        Task<User?> GetUserById(string? id);
        Task<IdentityResult> CreateUser(UserCreateModel model);
        bool IsValidRoles(string sRoles);
        Task<UserViewModel?> getUserAndRolesById(string? id);
        Task<IdentityResult?> updateUser(UserViewModel userViewModel);
        Task<IdentityResult?> deleteUser(string? id);
    }
}
