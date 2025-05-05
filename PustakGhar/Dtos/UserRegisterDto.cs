using System.ComponentModel.DataAnnotations;

namespace AlishPustakGhar.Dtos;

public class UserRegisterDto
{
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }


    public string? ImageURL { get; set; }

    
    public int Age { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public string Address { get; set; }
    
   

}