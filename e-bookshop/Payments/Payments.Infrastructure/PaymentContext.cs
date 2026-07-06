using Microsoft.EntityFrameworkCore;
using Payments.Domain;
using Payments.Infrastructure.Idempotency;

namespace Payments.Infrastructure;

public class PaymentsContext(DbContextOptions<PaymentsContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ProcessedMessage> ProcessedMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(p =>
        {
            p.HasKey(x => x.Id);
            p.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            p.Property(x => x.Status).HasConversion<string>();
            p.Property(x => x.FailureReason).IsRequired(false);
        });

        modelBuilder.Entity<ProcessedMessage>(p =>
        {
            p.HasKey(x => x.MessageId);
            p.Property(x => x.MessageType).HasMaxLength(200);
        });
    }
}