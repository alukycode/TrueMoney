namespace Bank.BankApi
{
    using System.Threading.Tasks;

    using Bank.BankEntities;

    public interface IBankApi
    {
        BankResponse Do(BankTransaction bankTransaction);

        BankResponse DoWithVisa(BankVisaTransaction bankTransaction);

        decimal? GetBalance(string accountNumber);
    }
}