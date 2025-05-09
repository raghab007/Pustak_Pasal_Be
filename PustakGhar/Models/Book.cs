using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlishPustakGhar.Models;

namespace AlishPustakGhar.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string ISBN { get; set; }
        public DateTime? PublishedDate { get; set; }
        
        public String Publisher { get; set; }
        public string Description { get; set; }
        
        public int Price { get; set; }
        
        public int Stock { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        public string FrontImage { get; set; }
        
        public string BackImage { get; set; }
        
        public int TotalSold { get; set; }
        
        
        public int TotalReviews { get; set; }
        
        public string ReleaseStatus { get; set; }

        public bool IsOnSale { get; set; } = false;
        
        public bool IsBestSeller { get; set; } = false;
        
        public  DateTime? DiscoundStartDate { get; set; }
        
        public  DateTime? DiscoundEndDate { get; set; }

        public int DiscountPercentage { get; set; } = 0;
        
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
        
        public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        
        
        public virtual ICollection<Favourites> Favourites { get; set; } = new List<Favourites>();
        public bool IsDiscountActive()
        {
            if (DiscountPercentage <= 0) return false;
    
            var now = DateTime.UtcNow;
            return DiscoundStartDate.HasValue 
                   && DiscoundEndDate.HasValue
                   && now >= DiscoundStartDate.Value
                   && now <= DiscoundEndDate.Value;
        }
    }
}