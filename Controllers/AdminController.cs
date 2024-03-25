using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;

        public AdminController(ILogger<AdminController> logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        //GET
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserViewModel> userList = await _adminService.GetAllUser();
            return View(userList);
        }

        //GET
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User? user = await _adminService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [ActionName("Create")]
        [HttpPost]
        public async Task<IActionResult> CreatePOSTAsync(UserCreateModel userCreateModel)
        {
            if (ModelState.IsValid)
            {
                if (!_adminService.IsValidRoles(userCreateModel.Roles))
                {
                    ModelState.AddModelError("Roles", "Roles must be Admin, Manager and/or User!");
                }

                var result = await _adminService.CreateUser(userCreateModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserViewModel? userViewModel = await _adminService.getUserAndRolesById(id);
            if (userViewModel == null && userViewModel.User == null)
            {
                return NotFound();
            }
            return View(userViewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPOSTAsync(UserViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return BadRequest();
            }
            var result = await _adminService.updateUser(userViewModel);
            if (result != null)
            {
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "There is an error occured.");
            }
            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(string? id)
        {
            var result = await _adminService.deleteUser(id);
            if (result != null)
            {
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "There is an error occured.");
            }
            return View();
        }
    }
}
