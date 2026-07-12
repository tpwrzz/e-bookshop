namespace Bookshop.Contracts.Messages;

public record PaymentProcessed(
    Guid OrderId,
    Guid UserId,
    DateTime ProcessedAt
);