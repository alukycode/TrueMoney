namespace Bank.BankApi
{
    using System.Threading.Tasks;

    using Bank.BankEntities;

    public class BankApi : IBankApi
    {
        public async Task<BankResponse> Do(BankTransaction bankTransaction)
        {
            //var bankData = new BankData().GetBankData();
            //todo - check and update bank date
            return BankResponse.Success;
        }

        public async Task<BankResponse> DoWithVisa(BankVisaTransaction bankTransaction)
        {
            return BankResponse.Success;
        }
    }
}