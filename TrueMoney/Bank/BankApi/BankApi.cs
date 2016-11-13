﻿namespace Bank.BankApi
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bank.BankEntities;
    using Bank.Resources;

    public class BankApi : IBankApi
    {
        public async Task<BankResponse> Do(BankTransaction bankTransaction)
        {
            //var bankData = new BankData().GetBankData();
            //todo - check and update bank date
            return BankResponse.Error;
        }

        public async Task<BankResponse> DoWithVisa(BankVisaTransaction bankTransaction)
        {
            var data = BankDataHelper.GetAccounts();
            var senderAccount =
                data.FirstOrDefault(
                    x =>
                    x.VisaNumber == bankTransaction.SenderCardNumber && 
                    x.VisaCcv == bankTransaction.SenderCcvCode
                    && x.VisaDate == bankTransaction.SenderValidBefore && 
                    x.VisaName == bankTransaction.SenderName);
            var receiverAccount = data.FirstOrDefault(
                x => x.BankAccountNumber == bankTransaction.RecipientAccountNumber);
            if (senderAccount == null || receiverAccount == null)
            {
                return BankResponse.Error;
            }

            if (senderAccount.Amount < bankTransaction.Amount)
            {
                return BankResponse.NotEnoughtMoney;
            }

            senderAccount.Amount -= bankTransaction.Amount;
            receiverAccount.Amount += bankTransaction.Amount;

            BankDataHelper.SaveAccounts(data);

            return BankResponse.Success;
        }
    }
}