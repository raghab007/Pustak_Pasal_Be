namespace AlishPustakGhar.Dtos;

public class BookAddDto
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    
    public DateTime PublishedDate { get; set; }
        
    public String Publisher { get; set; }
    public string Description { get; set; }
        
    public int Price { get; set; }
        
    public int Stock { get; set; }
    
    public IFormFile FrontImage { get; set; }
        
    public IFormFile BackImage { get; set; }
        
    public string ReleaseStatus { get; set; }

    public bool IsOnSale { get; set; } = false;
        
        
    public  DateTime DiscountStartDate { get; set; }
        
    public  DateTime DiscountEndDate { get; set; }
        
}