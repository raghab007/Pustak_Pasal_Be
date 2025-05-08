using System.ComponentModel.DataAnnotations.Schema;
using AlishPustakGhar.Models;

namespace WebApplication1.Model;

public class CartBook
{
    public Guid Id { get; set; }= Guid.NewGuid();
    [ForeignKey(nameof(Cart))]
    public Guid CartId { get; set; }
    public virtual Cart Cart { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    public virtual Book Book { get; set; }
    
    public int Quantity { get; set; }
    
}