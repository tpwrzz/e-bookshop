using Catalog.Domain;
using Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.Infrastructure
{
    public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(b =>
            {
                b.Property(book => book.Genre)
            .HasConversion(
        g => string.Join(',', g.Select(x => x.ToString())),
        g => g.Split(',', StringSplitOptions.RemoveEmptyEntries)
              .Select(x => Enum.Parse<Genre>(x))
              .ToList<Genre>())
    .HasColumnType("nvarchar(500)")
    .Metadata.SetValueComparer(new ValueComparer<ICollection<Genre>>(
        (c1, c2) => c1!.SequenceEqual(c2!),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList()));
                b.OwnsMany(book => book.Reviews, reviewBuilder =>
                {
                    reviewBuilder.WithOwner().HasForeignKey("BookId");
                    reviewBuilder.OwnsOne(review => review.Rating, ratingBuilder =>
                    {
                        ratingBuilder.Property(r => r.Value).HasColumnName("Rating");
                    });
                });

                b.OwnsOne(book => book.Price, moneyBuilder =>
                {
                    moneyBuilder.Property(m => m.Amount)
                        .HasColumnName("Price")
                        .HasColumnType("decimal(18,2)"); 
                    moneyBuilder.Property(m => m.Currency).HasColumnName("Currency");
                });
            });
        }
    }
}

