using BookShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookShoppingCart.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooks(string temp="", int GenreId = 0)
        {
            temp = temp.ToLower();
            IEnumerable<Book> books = await (from book in _context.Books
                         join genre in _context.Genres on book.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(temp) ||(book!=null && book.BookName.ToLower().StartsWith(temp)) 
                         
                         select new Book
                         {
                             Id = book.Id,
                             BookName = book.BookName,
                             Image = book.Image,
                             AuthorName = book.AuthorName,
                             GenreId = book.GenreId,
                             Price = book.Price,
                             GenreName = genre.GenreName,
                         }).ToListAsync();
            if (GenreId > 0)
            {
                books = books.Where(a=>a.GenreId == GenreId).ToList();
            }
            return books;
        }
    }
}
