using Bookshop.SharedKernel.Domain.Common.Enums;

namespace Bookshop.SharedKernel.Domain
{
    public class Money
    {
        public decimal Amount { get; }
        public Currency Currency { get; }
        public Money(decimal amount, Currency currency)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative");

            Amount = amount;
            Currency = currency;
        }
    }
}
