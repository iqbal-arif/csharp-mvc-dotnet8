using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;


namespace RetailWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        // Field to retrieve db object
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //Retriving all the Categories list
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(obj);    // Keeping track of what needs to Add
                _unitOfWork.Save();          // Creates a Category in Db
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
            // Method-2:First or Default Method
            // Method-3:Link Operation
            //Category? categoryFromDb2 = _db.Categories.Where( u => u.Id == id).FirstOrDefault(); 
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id); // Method-1: By ID only
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
                _unitOfWork.Category.Update(obj); ;    // Keeping track of what needs to be updated
                _unitOfWork.Save();           // Updates a Category in Db

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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id); // Method-1: By ID only


            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);    // Keeping track of what needs to be deleted
            _unitOfWork.Save();          // Updates a Category in Db
            TempData["success"] = "Category Deleted successfully";

            return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method


        }

    }
}
