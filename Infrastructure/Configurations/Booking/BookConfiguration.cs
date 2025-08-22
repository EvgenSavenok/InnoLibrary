using Domain.Entities.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Booking;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(book => book.Id);
        builder.Property(book => book.BookTitle).IsRequired().HasMaxLength(255);
        builder.Property(book => book.ISBN).IsRequired().HasMaxLength(13).HasColumnType("char(13)");
        builder.Property(book => book.GenreType).IsRequired();
        builder.Property(book => book.Description).IsRequired();
        builder.Property(book => book.Amount).IsRequired();
    }
}