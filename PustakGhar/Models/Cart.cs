
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Model;

namespace AlishPustakGhar.Models;

public class Cart
{
    
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<CartBook> CartBooks { get; set; } = new List<CartBook>();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}