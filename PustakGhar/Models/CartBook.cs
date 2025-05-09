using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AlishPustakGhar.Models;

namespace AlishPustakGhar.Model;

[Table("CartItems")]
public class CartItem
{
    [Key]
    public Guid Id { get; set; }= Guid.NewGuid();
    
    
    [ForeignKey(nameof(Cart))]
    public Guid CartId { get; set; }
    public virtual Cart Cart { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    public virtual Book Book { get; set; }
    
    public int Quantity { get; set; }
    
}