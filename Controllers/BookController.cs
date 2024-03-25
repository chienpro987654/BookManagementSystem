using BookManagementSystem.Filters;
using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BookManagementSystem.Controllers
{
    [TypeFilter(typeof(BookValidationActionFilter))]
    [AutoValidateAntiforgeryToken]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;


        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        // GET: Book
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index()
        {
            return View(await _bookService.GetAllBooksAsync());
        }

        // GET: Book/Details/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            BookCreateViewModel model = await _bookService.createViewModel();
            return View(model);
        }

        // POST: Book/Create
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isGood = true;
                if (viewModel.Book.ImageName.Equals(string.Empty) && viewModel.Image == null)
                {
                    ModelState.AddModelError("Image", "Image field is required");
                    isGood = false;
                }
                if (viewModel.Book.FileName.Equals(string.Empty) && viewModel.File == null)
                {
                    ModelState.AddModelError("File", "File field is required");
                    isGood = false;
                }
                if (!_bookService.IsValidImage(viewModel))
                {
                    ModelState.AddModelError("Image", "Invalid type for image");
                    isGood = false;
                }
                if (!_bookService.isValidFile(viewModel))
                {
                    ModelState.AddModelError("File", "Invalid type for file");
                    isGood = false;
                }
                var result = await _bookService.CreateBookAsync(viewModel);
                if (result != 0)
                {
                    TempData["success"] = "Author Created Successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            viewModel = await _bookService.createViewModel(viewModel);
            return View(viewModel);
        }



        // GET: Book/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetBookByIdAsync(id);
            BookCreateViewModel viewModel = await _bookService.createViewModel(null, id);
            if (book == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Book/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookCreateViewModel viewModel)
        {
            if (id != viewModel.Book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _bookService.UpdateBook(id, viewModel))
                    {
                        TempData["success"] = "Book Updated Successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_bookService.BookExists(viewModel.Book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(await _bookService.createViewModel(viewModel, id));
        }

        // GET: Book/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _bookService.DeleteBook(id) == false)
            {
                TempData["failed"] = "There is an error when trying to create object. Please try again.";

            }
            return RedirectToAction(nameof(Index));
        }

        [DisplayName("Details")]
        public async Task<IActionResult> DetailForUser(int id)
        {
            BookReadViewModel viewModel = await _bookService.CreateReadViewModel(id);
            return View(viewModel);
        }

        public async Task<IActionResult> Read(int id, int page)
        {
            BookReadViewModel viewModel = await _bookService.CreateReadViewModel(id, page);
            return View(viewModel);
        }
    }
}
