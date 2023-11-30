using BookShoppingCart.Areas.Admin.Models;
using BookShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace BookShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookAdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var listBook = _context.Books.
                Include(x => x.Genre).ToList();
            return View(listBook);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //BookViewModel bookAdmin = new BookViewModel();
            ViewBag.Genre = new SelectList(_context.Genres, "Id", "GenreName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book bookVm)
        {
            //Book book = new Book();
            if (!ModelState.IsValid)
            {
                var files = Request.Form.Files;
                byte[] photo = null;
                using (var fileStream = files[0].OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        photo = memoryStream.ToArray();
                    }
                }
                //book.BookName = bookVm.BookName;
                //book.AuthorName = bookVm.AuthorName;
                //book.Price = bookVm.Price;
                bookVm.Image = photo;
                //book.Image = photo;
                //book.GenreId = bookVm.GenreId;
                _context.Books.Add(bookVm);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            //book.Image = bookVm.Image;
            
            return View(bookVm);
        }
        [HttpGet]
        public IActionResult Edit(int id) 
        {
            ViewBag.Genre1 = new SelectList(_context.Genres, "Id", "GenreName");
            if (id == null)
            {
                return NotFound();
            }
            var edtBook = _context.Books.Where(x=>x.Id == id).Include(y=>y.Genre).FirstOrDefault();
            if (edtBook == null)
            {
                return NotFound();
            }
                return View(edtBook);
        }
        [HttpPost]
        public IActionResult Edit(Book edtBook)
        {
            
            var book = _context.Books.Where(x => x.Id == edtBook.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] photo = null;
                    using (var fileStream = files[0].OpenReadStream())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            photo = memoryStream.ToArray();
                        }
                    }
                    edtBook.Image = photo;
                }
                    book.BookName = edtBook.BookName;
                    book.AuthorName = edtBook.AuthorName;
                    book.Price = edtBook.Price;
                    book.Image = edtBook.Image;
                    book.GenreId = edtBook.GenreId;
                    _context.Books.Update(book);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
            }

            return View(book);
        }

        public ActionResult Delete(int id)
        {
            var book = _context.Books.Where(x=>x.Id ==id).FirstOrDefault();
            if(book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
