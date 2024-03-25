using BookManagementSystem.Data;
using BookManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;

        public AdminService(ApplicationDbContext db, UserManager<User> userManager, IUserStore<User> userStore)
        {
            _db = db;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
        }

        public IEnumerable<Book> GetListBook()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUser()
        {
            var userList = await _userManager.Users.ToListAsync();
            var userViewList = new List<UserViewModel>();
            foreach (var user in userList)
            {
                userViewList.Add(new UserViewModel
                {
                    User = user,
                    Roles = string.Join(",", _userManager.GetRolesAsync(user).Result.ToArray())
                });
            }
            return userViewList;
        }

        public async Task<User?> GetUserById(string? id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }

        public async Task addRolesForUserAsync(User user, string sRoles)
        {
            if (sRoles.Equals("User"))
            {
                IList<string> roleList = await _userManager.GetRolesAsync(user);
                roleList.Remove("User");
                await _userManager.RemoveFromRolesAsync(user, roleList);
            }
            foreach (string s in sRoles.Split(","))
            {
                string sTmp = s.Trim();
                var result = await _userManager.AddToRoleAsync(user, sTmp);
            }

        }

        public async Task<IdentityResult> CreateUser(UserCreateModel userCreateModel)
        {
            User user = new User
            {
                UserName = userCreateModel.Email,
                Email = userCreateModel.Email
            };
            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, userCreateModel.Password);
            if (result.Succeeded)
            {
                await addRolesForUserAsync(user, userCreateModel.Roles);
            }
            return result;
        }

        public bool IsValidRoles(string sRoles)
        {
            string[] arrRoles = sRoles.Split(",");
            foreach (string role in arrRoles)
            {
                if (!role.Equals("Admin") && !role.Equals("Manager") && !role.Equals("User"))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<UserViewModel?> getUserAndRolesById(string? id)
        {
            UserViewModel model = new UserViewModel();
            User? user = await GetUserById(id);
            if (user != null)
            {
                model.User = user;
                model.Roles = string.Join(",", _userManager.GetRolesAsync(user).Result.ToArray());
            }
            return model;
        }
        public async Task<IdentityResult?> updateUser(UserViewModel userViewModel)
        {
            User? user = await GetUserById(userViewModel.User.Id);
            if (user != null)
            {
                user.Email = userViewModel.User.Email;
                user.FullName = (userViewModel.User.FullName == null) ? string.Empty : userViewModel.User.FullName;
                user.Gender = userViewModel.User.Gender;
                user.BirthDate = userViewModel.User.BirthDate;
                user.PhoneNumber = userViewModel.User.PhoneNumber;
                user.UpdatedAt = DateTime.Now;
                userViewModel.Roles = (userViewModel.Roles == null) ? "User" : userViewModel.Roles;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await addRolesForUserAsync(user, userViewModel.Roles);
                }
                return result;
            }
            return null;
        }

        public async Task<IdentityResult?> deleteUser(string? id)
        {
            User? user = await GetUserById(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }
                return result;
            }
            return null;
        }
    }
}
