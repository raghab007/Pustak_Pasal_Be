using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlishPustakGhar.Model;

namespace AlishPustakGhar.Models;


[Table("Favourites")]
public class Favourites
{
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    
    [ForeignKey(nameof(User))]
    public Guid userId { get; set; }
    public virtual User User { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid bookId { get; set; }
    public virtual Book Book { get; set; }
}