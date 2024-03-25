using BookManagementSystem.Data;
using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorService _authorService;

        public AuthorController(ApplicationDbContext context, IAuthorService authorService)
        {
            _context = context;
            _authorService = authorService;
        }


        // GET: Author
        public async Task<IActionResult> Index()
        {
            return _context.Authors != null ?
                        View(await _context.Authors.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Authors'  is null.");
        }

        // GET: Author/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.Include(x => x.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Author/Create
        public IActionResult Create()
        {
            //IEnumerable<Category> categoryList = _context.Categories.ToList();
            //ViewBag.CategoryList = new SelectList(categoryList, "Id", "Name");
            AuthorCreateViewModel model = new AuthorCreateViewModel();
            model.Categories = _context.Categories.ToList();
            if (model.Categories == null)
            {
                RedirectToAction("DataNotAvailable", "Error", new { modelName = "Category" });
            }

            return View(model);
        }

        // POST: Author/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _authorService.createAuthorAsync(model) >= 0)
                {
                    TempData["success"] = "Author Created Successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            if (model.Categories == null)
            {
                model.Categories = _context.Categories.ToList();
            }
            return View(model);
        }

        // GET: Author/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            ////var author = await _context.Authors.FindAsync(id);
            //if (author == null)
            //{
            //    return NotFound();
            //}
            AuthorCreateViewModel viewModel = await _authorService.createViewModel(null, id);
            return View(viewModel);
        }

        // POST: Author/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorCreateViewModel viewModel)
        {
            if (id != viewModel.Author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _authorService.UpdateAuthor(id, viewModel))
                    {
                        TempData["success"] = "Book Updated Successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_authorService.AuthorExists(viewModel.Author.Id))
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
            return View(await _authorService.createViewModel(viewModel, id));
        }

        // GET: Author/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
