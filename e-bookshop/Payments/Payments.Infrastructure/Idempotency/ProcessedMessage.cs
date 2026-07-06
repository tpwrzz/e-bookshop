namespace Payments.Infrastructure.Idempotency;

public class ProcessedMessage
{
    public Guid MessageId { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}