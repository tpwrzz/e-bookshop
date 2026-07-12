using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payments.Infrastructure;

namespace Payments.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentsContext context) : ControllerBase
{
    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId)
    {
        var payment = await context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);

        if (payment is null)
            return NotFound($"No payment found for order {orderId}.");

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