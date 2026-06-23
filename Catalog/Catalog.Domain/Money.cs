using Catalog.Domain.Enums;

namespace Catalog.Domain
{
    public class Money
    {
        public double Amount { get; }
        public Currencies Currency { get; }
        public Money(double amount, Currencies currency)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative");

            Amount = amount;
            Currency = currency;
        }
    }
}
