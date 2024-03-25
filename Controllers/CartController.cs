using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ICartService _cartService;

        public CartController(ILogger<CartController> logger, UserManager<User> userManager, ICartService cartService)
        {
            _logger = logger;
            _userManager = userManager;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            return View();
        }
    }
}
