using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        [BindProperty]
        public Category Category { get; set; }

        public CreateModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            _dbContext.Categories.Add(Category);
            _dbContext.SaveChanges();
            TempData["success"] = "Category has been created successfully";

            return RedirectToPage("Index");
        }
    }
}
