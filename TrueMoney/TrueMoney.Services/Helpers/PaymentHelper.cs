namespace TrueMoney.Services.Helpers
{
    using System;

    using TrueMoney.Data.Entities;

    public static class PaymentHelper
    {
        public static decimal CalculateLiability(Payment payment, decimal extraMoney, decimal interestRate, int period)
        {
            if (payment.DueDate < DateTime.Now)
            {
                return Normolize((payment.Amount - extraMoney) * (DateTime.Now - payment.DueDate).Days * (interestRate / 100)
                        / period);
            }

            return 0;
        }

        public static decimal Normolize(decimal source)
        {
            if (source == 0)
            {
                return source;
            }

            return Convert.ToDecimal(Math.Floor(source * 100) + 1) / 100;
        }
    }
}