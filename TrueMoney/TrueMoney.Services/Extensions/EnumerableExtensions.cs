namespace TrueMoney.Services.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TrueMoney.Data.Entities;
    using TrueMoney.Services.Helpers;

    public static class EnumerableExtensions
    {
        public static IEnumerable<Payment> CalculateLiability(this IEnumerable<Payment> data, decimal extraMoney, Deal deal)
        {
            foreach (var payment in data.Where(x => !x.IsPaid).OrderBy(x => x.DueDate))
            {
                if (extraMoney > 0)
                {
                    payment.Liability = PaymentHelper.CalculateLiability(payment, extraMoney, deal.InterestRate, deal.DealPeriod);
                    extraMoney = 0;
                }
                else
                {
                    payment.Liability = PaymentHelper.CalculateLiability(payment, 0, deal.InterestRate, deal.DealPeriod);
                }
            }

            return data;
        }
    }
}