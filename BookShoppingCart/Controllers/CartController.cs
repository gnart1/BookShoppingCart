﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(int bookId, int quantity=1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(bookId, quantity);
            if (redirect == 0)
            {
                return Ok(cartCount); 
            }
                        
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepository.RemoveItem(bookId);
          
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> DeleteItem(int bookId)
        {
            var cartCount = await _cartRepository.DeleteItem(bookId);

            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemCart()
        {
            int cartItem = await _cartRepository.GetCartItemCount();
            return Ok(cartItem);
        }
        public async Task<IActionResult> CheckOut()
        {
            bool isCheckedOut = await _cartRepository.DoCheckOut();
            if (!isCheckedOut)
            {
                throw new Exception("Error");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
