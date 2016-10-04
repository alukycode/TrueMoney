namespace Bank.BankApi
{
    using Bank.BankEntities;

    public class BankApi : IBankApi
    {
        public BankResponse Do(BankTransaction bankTransaction)
        {
            var bankData = new BankData().GetBankData();
            //todo - check and update bank date
            return BankResponse.Error;
        }
    }
}