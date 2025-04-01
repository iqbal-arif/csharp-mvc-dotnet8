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

    public class CompanyController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;


        
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public CompanyController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        public IActionResult Index()
        {
            //Retriving all the Categories list
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            
            return View(objCompanyList); 
        }
        //Action Method for Create Categry
        public IActionResult UpSert(int? id) // Upsert = Up for Update sert for Insert
        {
            

            if (id == null || id == 0)
            {
                //Create
                return View(new Company());

            }
            else
            {
                //Update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id); //Returns only one Company
                return View(companyObj);

            }



        }

        [HttpPost]
        //public IActionResult Create(Company obj) // Relate to ViewBag & ViewData
        public IActionResult Upsert(Company CompanyObj)
        {
           

            if (ModelState.IsValid)
            {

                
                //Updating the Image if the Id is present
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }


                  
                //_unitOfWork.Company.Add(CompanyVM.Company);    // Keeping track of what needs to Add
                _unitOfWork.Save();          // Creates a Company in Db
                TempData["success"] = "Company Created successfully";
                return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
            }
            else
            {
               
                return View(CompanyObj);
            }

        }
        

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });

        }

        [HttpDelete]
        public IActionResult Delete(int? id) 
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(entity => entity.Id == id);

            if(CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

          

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
