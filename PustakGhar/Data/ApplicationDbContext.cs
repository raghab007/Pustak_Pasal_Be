using AlishPustakGhar.Model;
using AlishPustakGhar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlishPustakGhar.Data
{
    public class ApplicationDbContext :IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            :base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        
        public DbSet<Book> Books { get; set; }
        
        public DbSet<Author> Authors { get; set; }
        
        public DbSet<Genre> Genres { get; set; }
        
       public DbSet<Cart> Carts { get; set; }
       
       public DbSet<CartItem> CartItems { get; set; }
       
       public DbSet<Favourites> Favourites { get; set; }
       
       public DbSet<Order> Orders { get; set; }
       
       public DbSet<OrderItem> OrderItems { get; set; }
       
       public DbSet<Review> Reviews { get; set; }
    }
    
    
}