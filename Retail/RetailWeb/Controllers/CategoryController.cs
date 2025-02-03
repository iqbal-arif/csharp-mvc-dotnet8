using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using RetailWeb.Data;
using RetailWeb.Models;

namespace RetailWeb.Controllers
{
    public class CategoryController : Controller
    {
        // Field to retrieve db object
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //Retriving all the Categories list
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        //Action Method for Create Categry
        public IActionResult Create()
        {
            return View();
        }
    
    }
}
