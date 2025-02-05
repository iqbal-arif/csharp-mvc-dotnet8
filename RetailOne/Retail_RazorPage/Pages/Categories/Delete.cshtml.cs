using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Retail_RazorPage.Data;
using Retail_RazorPage.Models;

namespace Retail_RazorPage.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        // DB Property
        private readonly ApplicationDbContext _db;

        // Category List
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Category = _db.Categories.Find(id);
            }

        }

        public IActionResult OnPost()
        {
            Category? obj = _db.Categories.Find(Category.Id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);    // Keeping track of what needs to be deleted
            _db.SaveChanges();          // Updates a Category in Db
            TempData["success"] = "Category Deleted successfully";

            return RedirectToPage("Index");  //Redirect to "Index" through IActionResult Method
        }

    }
}
