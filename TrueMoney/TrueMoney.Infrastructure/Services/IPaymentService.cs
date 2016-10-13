﻿namespace TrueMoney.Infrastructure.Services
{
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface IPaymentService
    {
        Task<PaymentResult> LendMoney(int loanId, int payForId, float count, VisaDetails visaDetails);
        //Task<PaymentResult> PayLoanPart(int appId, int payerId, float count);
    }
}