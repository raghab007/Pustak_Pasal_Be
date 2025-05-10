using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlishPustakGhar.Model;

namespace AlishPustakGhar.Models;
[Table("Order")]
public class Order
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string OrderStatus { get; set; } = "Pending";
    
    public decimal TotalPrice { get; set; }
    
    // For discount requirements
    public decimal DiscountPercentage { get; set; } = 0;
    public bool IsDiscountApplied { get; set; } = false;
    
    // For claim code functionality
    public string ClaimCode { get; set; } = GenerateClaimCode();
    public DateTime? ClaimedDate { get; set; } = null;
    
    public  DateTime? ProcessedDate { get; set; }
    
    
    public DateTime? DeliveredDate { get; set; }
    
    
    // Foreign key to User
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    // Navigation property to User
    public virtual User User { get; set; }

    // Navigation property to OrderItems
    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    
    private static string GenerateClaimCode()
    {
        // Implement your claim code generation logic
        // Example: ALISH-{timestamp}-{random chars}
        return $"ALISH-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
    }
    
    public void ApplyDiscounts(int bookCount, int successfulOrderCount)
    {
        // Apply 5% discount if 5+ books
        if (bookCount >= 5)
        {
            DiscountPercentage += 5;
            IsDiscountApplied = true;
        }
        
        // Apply additional 10% discount if 10+ successful orders (stackable)
        if (successfulOrderCount >= 10)
        {
            DiscountPercentage += 10;
            IsDiscountApplied = true;
        }
        
        // Calculate final price with discounts
        if (IsDiscountApplied)
        {
            TotalPrice = TotalPrice * (1 - (DiscountPercentage / 100m));
        }
    }
}