using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlishPustakGhar.Model;

[Table("Author")]
public class Author
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Bio { get; set; }
    public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;
    public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();

}