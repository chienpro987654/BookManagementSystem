using BookManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookManagementSystem.Filters
{
    public class BookValidationActionFilter : IResourceFilter
    {
        private readonly ApplicationDbContext _db;

        public BookValidationActionFilter(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!_db.Categories.Any())
            {

                context.Result = new RedirectToActionResult("DataNotAvailable", "Error", new { modelName = "Category" });
            }
            if (!_db.Authors.Any())
            {
                context.Result = new RedirectToActionResult("DataNotAvailable", "Error", new { modelName = "Author" });
            }
        }
    }
}
