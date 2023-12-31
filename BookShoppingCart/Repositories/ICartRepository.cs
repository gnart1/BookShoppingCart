﻿namespace BookShoppingCart.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int bookId, int quantity);
        Task<int> RemoveItem(int bookId);
        Task<int> DeleteItem(int bookId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckOut();
    }
}
