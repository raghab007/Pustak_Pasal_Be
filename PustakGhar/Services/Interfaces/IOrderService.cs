using AlishPustakGhar.Model;
using AlishPustakGhar.Models;
using AlishPustakGhar.Models.DTOs;

namespace AlishPustakGhar.Services.Interfaces;

public interface IOrderService
{
    
    Task<Order> PlaceOrder(Guid userId, List<CartItem> cartItems);
    Task<bool> CancelOrder(Guid orderId, Guid userId);
    Task<bool> ProcessOrder(Guid orderId, string staffUserId);
    Task<bool> DeliverOrder(Guid orderId, string staffUserId);
    Task<Order> GetOrderDetails(Guid orderId, Guid userId);
    Task<List<Order>> GetUserOrders(Guid userId);

    Task<ClaimCodeDetailsDto> GetDetailsByClaimCode(string claimCode);

    public  Task<List<ClaimCodeDetailsDto>> GetAllOrdersWithUserDetails();




}