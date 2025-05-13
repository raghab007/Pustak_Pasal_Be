using AlishPustakGhar.Models;
using System.Threading.Tasks;
using AlishPustakGhar.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace AlishPustakGhar.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            var emailSettings = _configuration.GetSection("EmailSettings");
            _smtpServer = emailSettings["SmtpServer"];
            _smtpPort = int.Parse(emailSettings["SmtpPort"]);
            _smtpUsername = emailSettings["SmtpUsername"];
            _smtpPassword = emailSettings["SmtpPassword"];
            _fromEmail = emailSettings["FromEmail"];
        }

        public async Task SendOrderConfirmation(string email, Order order)
        {
            var subject = $"Order Confirmation - Order #{order.Id}";
            var body = $@"
                <h2>Thank you for your order!</h2>
                <p>Your order has been confirmed.</p>
                <p><strong>Order Details:</strong></p>
                <ul>
                    <li>Order ID: {order.Id}</li>
                    <li>Claim Code: {order.ClaimCode}</li>
                    <li>Total Amount: ${order.TotalPrice}</li>
                    <li>Order Date: {order.CreatedDate}</li>
                </ul>
                <p>Please keep your claim code safe. You'll need it to collect your books.</p>
                <p>Thank you for shopping with us!</p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendOrderCancellation(string email, Order order)
        {
            var subject = $"Order Cancellation - Order #{order.Id}";
            var body = $@"
                <h2>Order Cancellation Confirmation</h2>
                <p>Your order has been cancelled.</p>
                <p><strong>Order Details:</strong></p>
                <ul>
                    <li>Order ID: {order.Id}</li>
                    <li>Order Date: {order.CreatedDate}</li>
                    <li>Total Amount: ${order.TotalPrice}</li>
                </ul>
                <p>If you did not request this cancellation, please contact our support team immediately.</p>";

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            using var message = new MailMessage();
            message.From = new MailAddress(_fromEmail);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(_smtpServer, _smtpPort);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error but don't expose it to the client
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw new Exception("Failed to send email. Please try again later.");
            }
        }
    }
}