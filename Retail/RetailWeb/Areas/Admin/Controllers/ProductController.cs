using Microsoft.AspNetCore.Mvc;

namespace RetailWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
