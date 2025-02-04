using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Retail_RazorPage.Data;
using Retail_RazorPage.Models;

namespace Retail_RazorPage.Pages.Categories
{
    public class IndexModel : PageModel
    {
        // DB Property
        private readonly ApplicationDbContext _db;

        // Category List
        public List<Category> CategoryList { get; set; }


        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }

             
            
    }
}
