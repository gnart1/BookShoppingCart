using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCart.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext context,IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<int> AddItem(int bookId,int quantity)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try { 
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("chua dang nhap");
                    
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.ShoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                //cart detail
                var cartItem = _context.CartDetails
                            .FirstOrDefault(a=>a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    var book = _context.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = quantity,
                        UnitPrice = book.Price

                    };
                    _context.CartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int bookId)
        {
            //using var transaction = _context.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("chua dang nhap");
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Cart trong");
                }
                //cart detail
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if(cartItem == null)
                {
                    throw new Exception("khong co san pham");
                }
                else if (cartItem.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                   cartItem.Quantity= cartItem.Quantity - 1;
                }
                _context.SaveChanges();
                //transaction.Commit();
            }
            catch (Exception ex)
            {
                
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> DeleteItem(int bookId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("chua dang nhap");
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Giỏ hàng trống");
                }
                //cart detail
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem == null)
                {
                    throw new Exception("Không có sản phẩm");
                }
                else 
                {
                    _context.CartDetails.Remove(cartItem);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if(userId == null)
            {
                throw new Exception("Invalid userid");

            }
            var shoppingCart = await _context.ShoppingCarts.Include(a=>a.CartDetails)
                                .ThenInclude(a=>a.Book)
                                .ThenInclude(a => a.Genre)
                                .Where(a=>a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<int> GetCartItemCount(string userId="")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts 
                              join cartDetail in _context.CartDetails
                        on cart.Id equals cartDetail.ShoppingCartId
                        select new {cartDetail.Id}
                        ).ToListAsync();
            return data.Count;
        }
        public async Task<bool> DoCheckOut()
        {
            using var transaction = _context.Database.BeginTransaction(); 
            try
            {
                //di chuyển dữ liệu từ cartDetail sang order và orderdetail sau đó xóa cartdetail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Chua dang nhap");
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Gio hang trong");
                }
                var cartDetail = _context.CartDetails.Where(a =>a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                {
                    throw new Exception("Gio hang trong");
                }
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1, //pending

                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,

                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();

                //remove cartdetail
                _context.CartDetails.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
