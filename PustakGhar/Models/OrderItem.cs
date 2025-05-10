using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlishPustakGhar.Model;

namespace AlishPustakGhar.Models;


[Table("OrderItem")]
public class OrderItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    [ForeignKey(nameof(Order))]
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    public virtual Book Book { get; set; }
    
}