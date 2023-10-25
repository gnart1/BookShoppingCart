using BookShoppingCart.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenreAdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var listGenre = _context.Genres.ToList().Select(x => new GenreViewModel()
            {
                Id = x.Id,
                GenreName = x.GenreName
            }).ToList();
            return View(listGenre);
        }
        [HttpGet]
        public IActionResult Create()
        {
            GenreViewModel genre = new GenreViewModel();
            return View(genre);
        }
        [HttpPost]
        public IActionResult Create(GenreViewModel CrGenre)
        {
            Genre genre = new Genre();
            genre.GenreName = CrGenre.GenreName;
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vmGenre = _context.Genres.Where(x => x.Id == id)
                .Select(x => new GenreViewModel()
                {
                    Id =x.Id,
                    GenreName = x.GenreName
                }).FirstOrDefault();
            return View(vmGenre);
        }
        [HttpPost]
        public IActionResult Edit(GenreViewModel EdGenre)
        {
            if(ModelState.IsValid)
            {
                var genre = _context.Genres.FirstOrDefault(x=>x.Id == EdGenre.Id);
                if(genre != null)
                {
                    genre.GenreName = EdGenre.GenreName;
                    _context.Genres.Update(genre);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var genre = _context.Genres.Where(x=> x.Id == id).FirstOrDefault();
            if(genre != null)
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
