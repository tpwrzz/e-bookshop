using Payments.Domain;
using Payments.Domain.Repositories;

namespace Payments.Infrastructure.Repositories;

public class PaymentRepository(PaymentsContext context) : IPaymentRepository
{
    public async Task AddAsync(Payment payment)
    {
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();
    }
}