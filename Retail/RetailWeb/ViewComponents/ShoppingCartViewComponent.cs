using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Retail.DataAccess.Repository.IRepository;
using Retail.Utility;

namespace RetailWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult>InvokeAsync()
        {
            //Getting User ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            if (claim != null)
            {
                if(HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart,
                       _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
                }
                
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                //Clearing the Session
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
   
}
