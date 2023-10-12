using BookShoppingCart.Models;
using BookShoppingCart.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string temp = "", int genreId = 0)
        {
            
            IEnumerable<Book> books = await _homeRepository.GetBooks(temp, genreId);
            IEnumerable<Genre> genres = await _homeRepository.GetGenres();
            BookDisplayModel bookDisplayModel = new BookDisplayModel 
            { 
                Books = books,
                Genres = genres
            };
            return View(bookDisplayModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}