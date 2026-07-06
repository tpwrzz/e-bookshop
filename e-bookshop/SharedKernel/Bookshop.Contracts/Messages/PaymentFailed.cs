namespace BookShop.Contracts.Messages;

public record PaymentFailed(
    Guid OrderId,
    Guid UserId,
    string Reason,
    DateTime FailedAt
);