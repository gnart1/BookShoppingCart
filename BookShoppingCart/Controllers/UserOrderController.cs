﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;

        public UserOrderController(IUserOrderRepository userOrderRepository)
        {
            _userOrderRepository = userOrderRepository;
        }
        public async Task<IActionResult> UserOrders()
        {
            var order = await _userOrderRepository.UserOrders();
            return View(order);
        }
    }
}
