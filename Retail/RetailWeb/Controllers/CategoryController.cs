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
        [HttpPost]
         public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);    // Keeping track of what needs to Add
                _db.SaveChanges();          // Creates a Category in Db
                TempData["success"] = "Category Created successfully";
                return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
            }
            return View();

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
                //Add an Error page for Enduser
            }
            Category? categoryFromDb = _db.Categories.Find(id); // Method-1: By ID only
            // Method-2:First or Default Method
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            // Method-3:Link Operation
            //Category? categoryFromDb2 = _db.Categories.Where( u => u.Id == id).FirstOrDefault(); 
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
         public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);    // Keeping track of what needs to be updated
                _db.SaveChanges();          // Updates a Category in Db
                TempData["success"] = "Category Updated successfully";

                return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
            }
            return View();


        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
                //Add an Error page for Enduser
            }
            Category? categoryFromDb = _db.Categories.Find(id); // Method-1: By ID only
             
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);    // Keeping track of what needs to be deleted
            _db.SaveChanges();          // Updates a Category in Db
            TempData["success"] = "Category Deleted successfully";

            return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method


        }

    }
}
