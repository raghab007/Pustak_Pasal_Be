
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlishPustakGhar.Model;

namespace AlishPustakGhar.Models;

[Table("Cart")]
public class Cart
{
    
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}