namespace Bank.BankApi
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bank.BankEntities;
    using Bank.Resources;

    public class BankApi : IBankApi
    {
        public BankResponse Do(BankTransaction bankTransaction)
        {
            //var bankData = new BankData().GetBankData();
            //todo - check and update bank date
            return BankResponse.Error;
        }

        public BankResponse DoWithVisa(BankVisaTransaction bankTransaction)
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
            
            if (senderAccount == null)
            {
                return BankResponse.PermissionError;
            }

            if (receiverAccount == null)
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
        
        public decimal? GetBalance(string accountNumber)
        {
            var accounts = BankDataHelper.GetAccounts();

            return accounts.FirstOrDefault(x => x.BankAccountNumber == accountNumber)?.Amount;
        }
    }
}