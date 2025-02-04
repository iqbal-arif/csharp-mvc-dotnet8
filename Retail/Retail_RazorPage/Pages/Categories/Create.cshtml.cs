using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Retail_RazorPage.Data;
using Retail_RazorPage.Models;

namespace Retail_RazorPage.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        // DB Property
        private readonly ApplicationDbContext _db;

        // Category List
        public Category Category { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost( )
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["success"] = "Category Created successfully";
            return RedirectToPage("Index");
        }
    }
}
