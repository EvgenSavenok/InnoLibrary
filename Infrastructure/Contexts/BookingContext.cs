using Domain.Entities.Booking;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class BookingContext(DbContextOptions<BookingContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    
    public DbSet<Author> Authors { get; set; }
    
    public DbSet<UserBookReservation>  UserBookReservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingContext).Assembly);
        
        modelBuilder.Entity<Book>()
            .HasMany(t => t.BookAuthors)  
            .WithMany(t => t.BookAuthors) 
            .UsingEntity<Dictionary<string, object>>(
                "BookAuthor",
                j => j.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId"));
    }
}