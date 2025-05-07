using System.ComponentModel.DataAnnotations;

namespace AlishPustakGhar.Dtos;

public class AuthorDto
{
    public Guid? Id { get; set; }= Guid.NewGuid();
    
    [Required]
    public string Name { get; set; }
    
    public string Bio { get; set; }
    
}