namespace Bookshop.Contracts.Messages
{
    public record OrderPlaced(Guid OrderId, Guid UserId, decimal TotalAmount, string Currency, DateTime PlacedAt);
}
