using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.JSInterop;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;
using Retail.Models.ViewModels;
using Retail.Utility;

namespace RetailWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;

        //Injecting IWeHostEnvironment using Dependency Injection
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        //public ProductController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        public IActionResult Index()
        {
            //Retriving all the Categories list
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
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
                //Create
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

                //Checking file in the root folder, WebRoothPath gives www root folder
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) 
                { 
                    //Creating a new filename and combining the extension of the actual uploaded fiel
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //Gets the path of the file so we can define the location for file to be uploaded to
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    //Checking if the Image is available
                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //Delete the Old Image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //Updating the Image if the Id is present
                    if (productVM.Product.Id == 0)
                    {
                        _unitOfWork.Product.Add(productVM.Product);
                    }
                    else
                    {
                        _unitOfWork.Product.Update(productVM.Product);
                    }


                    //Copying the file
                    using (var fileStream = new FileStream(Path.Combine(productPath,filename), FileMode.Create))
                    {
                            file.CopyTo(fileStream); // copying the file to FileStream
                    }
                    //Saving it to Product Image URL
                    productVM.Product.ImageUrl = @"\images\product\" + filename;

                }

                //Another way of Checking the Id and comparing it with the Database Id
                //var product = _unitOfWork.Product.Get(x => x.Id == productVM.Product.Id);
                //if (product == null)

                    if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                } 
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                //_unitOfWork.Product.Add(productVM.Product);    // Keeping track of what needs to Add
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

        //Deleted the previous Delete function as It is operated under API Class Section

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //        //Add an Error page for Enduser
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id); // Method-1: By ID only


        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);    // Keeping track of what needs to be deleted
        //    _unitOfWork.Save();          // Updates a Product in Db
        //    TempData["success"] = "Product Deleted successfully";

        //    return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method

        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return Json(new { data = objProductList });

        }

        [HttpDelete]
        public IActionResult Delete(int? id) 
        {
            var productToBeDeleted = _unitOfWork.Product.Get(entity => entity.Id == id);

            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = 
                        Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
