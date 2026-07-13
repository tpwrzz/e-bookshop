using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payments.Infrastructure;
using Serilog.Core;

namespace Payments.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentsContext context, ILogger<PaymentsController> logger) : ControllerBase
{
    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId)
    {
        var payment = await context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);

        if (payment is null)
        {
            logger.LogWarning("No payment found for order {OrderId}", orderId);
            return NotFound($"No payment found for order {orderId}.");
        }

        logger.LogInformation("Retrieved payment {PaymentId} for order {OrderId}", payment.Id, orderId);

        return Ok(new
        {
            payment.Id,
            payment.OrderId,
            payment.UserId,
            payment.Amount,
            Status = payment.Status.ToString(),
            payment.FailureReason,
            payment.CreatedAt
        });
    }
}