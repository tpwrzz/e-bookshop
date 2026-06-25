using Microsoft.EntityFrameworkCore;
using Ordering.Domain;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure
{
    public class OrderingContext(DbContextOptions<OrderingContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(o =>
            {
                o.HasKey(x => x.Id);

                o.Property(x => x.OrderStatus)
                    .HasConversion<string>();

                o.OwnsOne(x => x.TotalCost, money =>
                {
                    money.Property(m => m.Amount)
                        .HasColumnName("TotalCost");

                    money.Property(m => m.Currency)
                        .HasColumnName("Currency");
                });

                o.OwnsOne(x => x.Address, address =>
                {
                    address.Property(a => a.Street);
                    address.Property(a => a.City);
                    address.Property(a => a.Country);
                    address.Property(a => a.Postcode);
                });

                o.OwnsMany(x => x.OrderItems, item =>
                {
                    item.WithOwner().HasForeignKey("OrderId");

                    item.OwnsOne(i => i.Price, money =>
                    {
                        money.Property(m => m.Amount)
                            .HasColumnName("Price");

                        money.Property(m => m.Currency)
                            .HasColumnName("Currency");
                    });
                });
            });
        }
    }
}

