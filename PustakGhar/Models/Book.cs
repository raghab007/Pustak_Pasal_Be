using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string ISBN { get; set; }
        public DateTime PublishedDate { get; set; }
        
        public String Publisher { get; set; }
        
        public int Price { get; set; }
        
        public int Quantity { get; set; }
        
        public string ImageURL { get; set; }
        
        public int TotalSold { get; set; }
        
        public int TotalRating { get; set; }
        
        public string Description { get; set; }
        
        public string TotalReviews { get; set; }
        
        public string ReleaseStatus { get; set; }

        public bool IsOnSale { get; set; } = false;
        
        public bool IsBestSeller { get; set; } = false;
        
        public  DateTime DiscoundStartDate { get; set; }
        
        public  DateTime DiscoundEndDate { get; set; }
        
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
        

    }
}