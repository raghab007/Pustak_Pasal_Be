using AlishPustakGhar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlishPustakGhar.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IBookService _bookService;

        public OrdersController(IOrderService orderService, ICartService cartService, IBookService bookService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _bookService = bookService;
            
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var cartItems = await _cartService.GetCartItems(userId);

            if (cartItems == null || !cartItems.Any())
                return BadRequest("Your cart is empty");

            try
            {
                var order = await _orderService.PlaceOrder(userId, cartItems);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            
            var result = await _orderService.CancelOrder(orderId, userId);
            if (!result) return BadRequest("Unable to cancel order");

            return Ok("Order cancelled successfully");
        }

      //  [Authorize(Roles = "Staff")]
        [HttpPost("{orderId}/process")]
        public async Task<IActionResult> ProcessOrder(Guid orderId)
        {
            var staffUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            
            var result = await _orderService.ProcessOrder(orderId, staffUserId);
            if (!result) return BadRequest("Unable to process order");

            return Ok("Order is now being processed");
        }

       // [Authorize(Roles = "Staff")]
        [HttpPost("{orderId}/deliver")]
        public async Task<IActionResult> DeliverOrder(Guid orderId)
        {
            var staffUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            
            var result = await _orderService.DeliverOrder(orderId, staffUserId);
            if (!result) return BadRequest("Unable to mark order as delivered");

            return Ok("Order marked as delivered");
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(Guid orderId)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            
            var order = await _orderService.GetOrderDetails(orderId, userId);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            
            var orders = await _orderService.GetUserOrders(userId);
            return Ok(orders);
        }

        [HttpGet("purchased-books")]
        public async Task<IActionResult> GetPurchasedBooks()
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            
            var books = await _bookService.GetPurchasedBooks(userId);
            return Ok(books);
        }
        
        [AllowAnonymous]
        [HttpGet("claim/{claimCode}")]
        public async Task<IActionResult> GetOrderByClaimCode(string claimCode)
        {
            var orderDetails = await _orderService.GetDetailsByClaimCode(claimCode);
            if (orderDetails == null)
                return NotFound("Order not found with the provided claim code");

            return Ok(orderDetails);
        }

        
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersWithUserDetails();
            return Ok(orders);
        }
    }
}