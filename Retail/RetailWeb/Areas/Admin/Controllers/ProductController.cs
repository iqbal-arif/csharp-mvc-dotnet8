using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;

namespace RetailWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //Retriving all the Categories list
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            
            return View(objProductList); 
        }
        //Action Method for Create Categry
        public IActionResult Create()
        {
            //Retriving IEnumberable of Category, now need to convert it to IEnumberable of SelectListItem     
            //IEnumerable<<SelectListItem> CategoryList = _unitOfWork.Category.GetAll(); 
            // Passing list of Products; one Product object. We also need list of Categories
            //Projections in EF converts the Category to IEnumberable of SelectList Item
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            //ViewBag Method

            //ViewBag.CategoryList = CategoryList;

            //ViewData Method
            ViewData["CategoryList"] = CategoryList; // Assigning the value of ViewData

            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
           

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);    // Keeping track of what needs to Add
                _unitOfWork.Save();          // Creates a Product in Db
                TempData["success"] = "Product Created successfully";
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
            //Product? productFromDb2 = _db.Categories.Where( u => u.Id == id).FirstOrDefault(); 
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id); // Method-1: By ID only
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj); ;    // Keeping track of what needs to be updated
                _unitOfWork.Save();           // Updates a Product in Db

                TempData["success"] = "Product Updated successfully";

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
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id); // Method-1: By ID only


            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);    // Keeping track of what needs to be deleted
            _unitOfWork.Save();          // Updates a Product in Db
            TempData["success"] = "Product Deleted successfully";

            return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method


        }
    }
}
