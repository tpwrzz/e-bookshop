namespace Payments.Domain.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
}