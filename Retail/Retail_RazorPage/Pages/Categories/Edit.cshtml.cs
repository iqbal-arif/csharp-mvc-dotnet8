using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Retail_RazorPage.Data;
using Retail_RazorPage.Models;

namespace Retail_RazorPage.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        // DB Property
        private readonly ApplicationDbContext _db;

        // Category List
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext db)
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
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);    // Keeping track of what needs to be updated
                _db.SaveChanges();          // Updates a Category in Db
                TempData["success"] = "Category Updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
        
    }
}
