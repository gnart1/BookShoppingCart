using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
