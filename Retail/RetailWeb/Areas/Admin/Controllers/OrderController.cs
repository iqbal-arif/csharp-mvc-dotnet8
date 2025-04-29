using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;
using Retail.Models.ViewModels;
using Retail.Utility;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;

namespace RetailWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
   
        public IActionResult Index()
        {
            return View();
        }
        
        //Order Details Method
        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties:"ApplicationUser"),
                orderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties:"Product")

            };

            return View(OrderVM);
        }

        //Details Post Action Method
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
        
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully.";

            return RedirectToAction(nameof(Details), new {orderId = orderHeaderFromDb.Id});
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(Details), new {orderId = OrderVM.OrderHeader.Id});
        }
        
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if(orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction(nameof(Details), new {orderId = OrderVM.OrderHeader.Id});
        }



        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);

            }

            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

        }

        //Payment for the Pending Orders for Company USERS
        [ActionName("Details")]
        [HttpPost]
        public IActionResult Details_Pay_Now()
        {
            //Order Header and Order Details
            OrderVM.OrderHeader = _unitOfWork.OrderHeader
                .Get(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
            OrderVM.orderDetail = _unitOfWork.OrderDetail
                .GetAll(u => u.OrderHeaderId == OrderVM.OrderHeader.Id, includeProperties: "Product");

            //Stripe Logic

            //It is a Regular Customer Account and we need to capture payment
            //Stripe Logic

            //Variable for URL
            var domain = "https://localhost:7113/";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                //URl = doamin + navigate to Customer Area , Cart Controller, and Action OrderConfirmation
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",

                //Redirecting to Index Page Customer Shopping Cart for Canncelled URL
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",

                //All Product Detail in LineItems
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),

                Mode = "payment",
            };

            //Generating Item List; if 10 items in the Cart it will iterate 10 times

            foreach (var item in OrderVM.orderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions //this is build-in class in stripe package
                {
                    //Creating new Price Object with PriceData
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        //SessionLineItemPriceDataProductDataOptions build-in class in stripe packag
                        //Gives the Name, Description, and Images of each Item

                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    //Quantity per Item
                    Quantity = item.Count
                };

                options.LineItems.Add(sessionLineItem);
            }



            var service = new Stripe.Checkout.SessionService();

            Session session = service.Create(options);

            //Update PaymentIntentId before Redirecting to the page
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);

            _unitOfWork.Save();

            //Redirecting to new URL
            Response.Headers.Add("Location", session.Url); //Provided by Stripe
            return new StatusCodeResult(303); //Meaning redirecting to new URL


        }


        //Redirecting for Compnay User

        public IActionResult PaymentConfirmation(int orderHeaderId)
        {
            //Based on Id retriving the Order Header
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderHeaderId);

            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                //This is an Order by Company

                var service = new SessionService();

                //Getting Payment status for Stripe Build-in SessionService class

                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    //Update PaymentIntentId before Redirecting to the page
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);

                    _unitOfWork.Save();
                }
            }

           

            return View(orderHeaderId);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders; 

            //Only Admin Can take tests
            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee)){
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");


            }

            //Filtering the Status after passed through GeAlla function as parameter
            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = objOrderHeaders });
        }

        #endregion
    }

}


   
