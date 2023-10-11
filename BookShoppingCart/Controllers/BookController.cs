using BookShoppingCart.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Index()
        {
            var ListBook = _context.Books.ToList();
            return View(ListBook);
        }
    }
}
