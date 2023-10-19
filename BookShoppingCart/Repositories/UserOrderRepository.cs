using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCart.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserOrderRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if(string.IsNullOrEmpty(userId))
            {
                throw new Exception("chua dang nhap tai khoan");
            }
            var orders = await _context.Orders.Include(x=>x.OrderDetail)
                        .ThenInclude(x=>x.Book)
                        .ThenInclude(x => x.Genre)
                        .Where(a=>a.UserId == userId).ToListAsync();
            return orders;
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
