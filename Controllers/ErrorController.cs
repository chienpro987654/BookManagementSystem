using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DataNotAvailable(string modelName)
        {
            TempData["modelName"] = modelName;
            return View();
        }
    }
}
