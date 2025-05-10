using AlishPustakGhar.Data;
using AlishPustakGhar.Models;
using AlishPustakGhar.Model;
using AlishPustakGhar.Models.DTOs;
using AlishPustakGhar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlishPustakGhar.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public OrderService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Order> PlaceOrder(Guid userId, List<CartItem> cartItems)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var user = await _context.Users
                    .Include(u => u.Orders)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) throw new Exception("User not found");

                // Calculate total price and check stock
                decimal totalPrice = 0;
                var orderItems = new List<OrderItem>();
                var booksToUpdate = new List<Book>();

                foreach (var cartItem in cartItems)
                {
                    var book = await _context.Books.FindAsync(cartItem.BookId);
                    if (book == null) throw new Exception($"Book {cartItem.BookId} not found");
                    if (book.Stock < cartItem.Quantity) throw new Exception($"Not enough stock for {book.Title}");

                    // Apply book discount if available
                    decimal itemPrice = book.Price;
                    if (book.IsDiscountActive())
                    {
                        itemPrice = book.Price * (1 - (book.DiscountPercentage / 100m));
                    }

                    totalPrice += itemPrice * cartItem.Quantity;
                    
                    orderItems.Add(new OrderItem
                    {
                        BookId = book.Id,
                        Quantity = cartItem.Quantity,
                        Price = itemPrice
                    });

                    // Prepare stock update
                    book.Stock -= cartItem.Quantity;
                    book.TotalSold += cartItem.Quantity;
                    booksToUpdate.Add(book);
                }

                // Create order
                var order = new Order
                {
                    UserId = userId,
                    TotalPrice = totalPrice,
                    OrderItems = orderItems
                };

                // Apply order-level discounts
                order.ApplyDiscounts(
                    bookCount: cartItems.Sum(ci => ci.Quantity),
                    successfulOrderCount: user.SuccessfulOrderCount
                );

                _context.Orders.Add(order);
                
                // Update books stock
                _context.Books.UpdateRange(booksToUpdate);
                
                // Clear cart
                _context.CartItems.RemoveRange(cartItems);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Send confirmation email with claim code
                await _emailService.SendOrderConfirmation(user.Email, order);

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CancelOrder(Guid orderId, Guid userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return false;
            if (order.OrderStatus != "Pending") return false;

            // Restore stock
            foreach (var item in order.OrderItems)
            {
                item.Book.Stock += item.Quantity;
                item.Book.TotalSold -= item.Quantity;
            }

            order.OrderStatus = "Cancelled";
            await _context.SaveChangesAsync();

            // Send cancellation email
            await _emailService.SendOrderCancellation(order.User.Email, order);

            return true;
        }

        public async Task<bool> ProcessOrder(Guid orderId, string staffUserId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.OrderStatus != "Pending") return false;

            order.OrderStatus = "Processing";
            order.ProcessedDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeliverOrder(Guid orderId, string staffUserId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.OrderStatus != "Processing") return false;

            order.OrderStatus = "Delivered";
            order.DeliveredDate = DateTime.UtcNow;
            
            // Increment user's successful order count
            order.User.SuccessfulOrderCount++;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> GetOrderDetails(Guid orderId, Guid userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public async Task<List<Order>> GetUserOrders(Guid userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }
        
        
        public async Task<ClaimCodeDetailsDto> GetDetailsByClaimCode(string claimCode)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.ClaimCode == claimCode);

            if (order == null)
                return null;

            return new ClaimCodeDetailsDto
            {
                Order = new OrderInfoDto
                {
                    Id = order.Id,
                    OrderDate = order.CreatedDate,
                    Status = order.OrderStatus,
                    TotalPrice = order.TotalPrice,
                    DiscountApplied = order.DiscountPercentage,
                    ClaimCode = order.ClaimCode
                },
                User = new UserInfoDto
                {
                    Name = $"{order.User.FirstName} {order.User.LastName}",
                    Email = order.User.Email,
                    Phone = order.User.PhoneNumber
                },
                Books = order.OrderItems.Select(oi => new BookInfoDto
                {
                    Id = oi.Book.Id,
                    Title = oi.Book.Title,
                    ISBN = oi.Book.ISBN,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    ImageUrl = oi.Book.FrontImage
                }).ToList()
            };
        }
        
        public async Task<List<ClaimCodeDetailsDto>> GetAllOrdersWithUserDetails()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();

            return orders.Select(o => new ClaimCodeDetailsDto
            {
                Order = new OrderInfoDto
                {
                    Id = o.Id,
                    OrderDate = o.CreatedDate,
                    Status = o.OrderStatus,
                    TotalPrice = o.TotalPrice,
                    DiscountApplied = o.DiscountPercentage,
                    ClaimCode = o.ClaimCode
                },
                User = new UserInfoDto
                {
                    Name = $"{o.User.FirstName} {o.User.LastName}",
                    Email = o.User.Email,
                    Phone = o.User.PhoneNumber
                },
                Books = o.OrderItems.Select(oi => new BookInfoDto
                {
                    Id = oi.Book.Id,
                    Title = oi.Book.Title,
                    ISBN = oi.Book.ISBN,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    ImageUrl = oi.Book.FrontImage
                }).ToList()
            }).ToList();
        }

        
        

    }
    
}