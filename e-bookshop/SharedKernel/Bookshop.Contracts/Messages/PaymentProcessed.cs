namespace BookShop.Contracts.Messages;

public record PaymentProcessed(
    Guid OrderId,
    Guid UserId,
    DateTime ProcessedAt
);