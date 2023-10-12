namespace BookShoppingCart.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> GetBooks(string temp = "", int GenreId = 0);
        Task<IEnumerable<Genre>> GetGenres();
    }
}
