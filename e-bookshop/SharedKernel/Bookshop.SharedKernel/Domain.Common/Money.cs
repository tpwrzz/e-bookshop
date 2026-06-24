using Bookshop.SharedKernel.Domain.Enums;

namespace Bookshop.SharedKernel.Domain
{
    public class Money
    {
        public double Amount { get; }
        public Currency Currency { get; }
        public Money(double amount, Currency currency)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative");

            Amount = amount;
            Currency = currency;
        }
    }
}
