using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet(int? id)
        {
            if (id != null || id != 0)
            {
                // Find работает только для Primary Key
                Category = _dbContext.Categories.Find(id)!;
            }
        }

        public IActionResult OnPost()
        {
            Category category = _dbContext.Categories.Find(Category.Id)!;

            if (category == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            TempData["success"] = "Category has been deleted successfully";

            return RedirectToPage("Index");
        }
    }
}
