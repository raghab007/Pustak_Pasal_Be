using AlishPustakGhar.Models;

namespace AlishPustakGhar.Services.Interfaces;
public interface IEmailService
{
    Task SendOrderConfirmation(string email, Order order);
    Task SendOrderCancellation(string email, Order order);
}