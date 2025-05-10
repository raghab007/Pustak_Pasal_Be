using AlishPustakGhar.Models;
using System.Threading.Tasks;
using AlishPustakGhar.Services.Interfaces;

namespace AlishPustakGhar.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendOrderConfirmation(string email, Order order)
        {
            // In a real implementation, this would send an actual email
            // For now, we'll just log to console
            Console.WriteLine($"Sending order confirmation to {email}");
            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Claim Code: {order.ClaimCode}");
            Console.WriteLine($"Total: {order.TotalPrice}");
            
            await Task.CompletedTask;
        }

        public async Task SendOrderCancellation(string email, Order order)
        {
            // In a real implementation, this would send an actual email
            Console.WriteLine($"Sending order cancellation to {email}");
            Console.WriteLine($"Order ID: {order.Id}");
            
            await Task.CompletedTask;
        }
    }
}