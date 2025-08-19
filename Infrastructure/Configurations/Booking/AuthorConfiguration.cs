using Domain.Entities.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Booking;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(author => author.AuthorId);
        builder.Property(author => author.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(author => author.LastName).IsRequired().HasMaxLength(100);
    }
}