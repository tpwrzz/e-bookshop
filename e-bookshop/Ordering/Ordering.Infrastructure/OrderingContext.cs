using Microsoft.EntityFrameworkCore;
using Ordering.Domain;
using Ordering.Domain.Enums;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Outbox;

namespace Ordering.Infrastructure
{
    public class OrderingContext(DbContextOptions<OrderingContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<ProcessedMessage> ProcessedMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(o =>
            {
                o.HasKey(x => x.Id);

                o.Property(x => x.OrderStatus)
                    .HasConversion<string>();

                o.OwnsOne(x => x.TotalCost, moneyBuilder =>
                {
                    moneyBuilder.Property(m => m.Amount)
                        .HasColumnName("Price")
                        .HasColumnType("decimal(18,2)");  // add this
                    moneyBuilder.Property(m => m.Currency).HasColumnName("Currency");
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

                    item.OwnsOne(i => i.Price, moneyBuilder =>
                    {
                        moneyBuilder.Property(m => m.Amount)
                            .HasColumnName("Price")
                            .HasColumnType("decimal(18,2)");
                    });
                });
            });
            modelBuilder.Entity<OutboxMessage>(o =>
            {
                o.HasKey(x => x.Id);
                o.Property(x => x.Payload).HasColumnType("nvarchar(max)");
                o.Property(x => x.ProcessedAt).IsRequired(false);
            });
            modelBuilder.Entity<ProcessedMessage>(p =>
            {
                p.HasKey(x => x.MessageId);
                p.Property(x => x.MessageType).HasMaxLength(200);
            });
        }
    }
}

