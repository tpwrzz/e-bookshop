using Payments.Domain.Enums;

namespace Payments.Domain;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? FailureReason { get; private set; }
    private Payment() { }

    public static Payment Process(Guid orderId, Guid userId, decimal amount)
    {
        var status = amount > 9999
            ? PaymentStatus.Failed
            : PaymentStatus.Succeeded;

        return new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            UserId = userId,
            Amount = amount,
            Status = status,
            CreatedAt = DateTime.UtcNow,
            FailureReason = status == PaymentStatus.Failed
                ? $"Amount {amount} exceeds maximum allowed transaction value."
                : null
        };
    }

    public bool Succeeded => Status == PaymentStatus.Succeeded;
}