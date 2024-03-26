using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel viewModel = await _homeService.CreateHomeViewModelAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Search(string query, List<string> checkedBoxes, string sortOrder)
        {
            HomeSearchViewModel viewModel = await _homeService.createHomeSearchViewModel(query, checkedBoxes);
            if (viewModel.BookResults != null)
            {
                viewModel.BookResults = _homeService.getOrderBookList(viewModel.BookResults, sortOrder);
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Category(string sortOrder, int CategoryId = -1)
        {
            HomeViewModel viewModel = await _homeService.createBookViewModelFromCategory(CategoryId, sortOrder);
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
