using System.ComponentModel.DataAnnotations;

namespace AlishPustakGhar.Dtos;

public class BulkDiscountDto
{
    [Required]
    public List<Guid> BookIds { get; set; }
    
    [Required]
    [Range(0, 100)]
    public int DiscountPercentage { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public bool SetOnSale { get; set; } = true;
}