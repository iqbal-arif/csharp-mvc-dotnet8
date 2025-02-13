using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;
using Retail.Models.ViewModels;

namespace RetailWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _weHostEnironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment weHostEnironment)
        {
            _unitOfWork = unitOfWork;
            _weHostEnironment = weHostEnironment;
        }
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
        public IActionResult UpSert(int? id) // Upsert = Up for Update sert for Insert
        {
            //START - HERE : OTHER METHODS, VIEWBAG/VIEWDATA
            //Retriving IEnumberable of Category, now need to convert it to IEnumberable of SelectListItem     
            //IEnumerable<<SelectListItem> CategoryList = _unitOfWork.Category.GetAll(); 
            // Passing list of Products; one Product object. We also need list of Categories
            //Projections in EF converts the Category to IEnumberable of SelectList Item
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString(),
            //});

            //ViewBag Method

            //ViewBag.CategoryList = CategoryList;

            //ViewData Method
            //ViewData["CategoryList"] = CategoryList; // Assigning the value of ViewData

            //return View();
            //END - HERE : OTHER METHODS, VIEWBAG/VIEWDATA

            //Product ViewModel Method
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),

                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //Creat
                return View(productVM);

            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id); //Returns only one product
                return View(productVM);

            }




        }
        [HttpPost]
        //public IActionResult Create(Product obj) // Relate to ViewBag & ViewData
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
           

            if (ModelState.IsValid)
            {

                //Checking file in the root folder
                string wwwRootPath = _weHostEnironment.WebRootPath;
                if (file != null) 
                { 
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                }

                _unitOfWork.Product.Add(productVM.Product);    // Keeping track of what needs to Add
                _unitOfWork.Save();          // Creates a Product in Db
                TempData["success"] = "Product Created successfully";
                return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
            }
            else
            {
                //Populates the list if the ModelState is FALSE. So the page is not borken
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
                return View(productVM);
            }

        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //        //Add an Error page for Enduser
        //    }
        //    // Method-2:First or Default Method
        //    // Method-3:Link Operation
        //    //Product? productFromDb2 = _db.Categories.Where( u => u.Id == id).FirstOrDefault(); 
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id); // Method-1: By ID only
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj); ;    // Keeping track of what needs to be updated
        //        _unitOfWork.Save();           // Updates a Product in Db

        //        TempData["success"] = "Product Updated successfully";

        //        return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
        //    }
        //    return View();


        //}

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
