using System;
using Bank.BankEntities;

namespace Bank.BankApi
{
    public class SimpleBankApi : IBankApi
    {
        public BankResponse Do(BankTransaction bankTransaction)
        {
            return BankResponse.Success;
        }

        public BankResponse DoWithVisa(BankVisaTransaction bankTransaction)
        {
            return BankResponse.Success;
        }

        public decimal GetBalance(string accountNumber)
        {
            return decimal.MaxValue;
        }
    }
}