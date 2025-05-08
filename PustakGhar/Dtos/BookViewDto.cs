
using AlishPustakGhar.Model;

namespace AlishPustakGhar.Dtos;

public class BookViewDto
{
    
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    
    public DateTime? PublishedDate { get; set; }
        
    public String Publisher { get; set; }
    public string Description { get; set; }
        
    public int Price { get; set; }
        
    public int Stock { get; set; }
    
    public string ReleaseStatus { get; set; }

    public bool IsOnSale { get; set; }
    
    public string FrontImage { get; set; }


    public BookViewDto(Book book)
    {
        Id = book.Id;
        Title = book.Title;
        ISBN = book.ISBN;
        PublishedDate = book.PublishedDate;
        Publisher = book.Publisher;
        Description = book.Description;
        Price = book.Price;
        Stock = book.Stock;
        ReleaseStatus = book.ReleaseStatus;
        IsOnSale = book.IsOnSale;
        FrontImage = book.FrontImage;
    }

}