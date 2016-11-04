namespace Bank.BankApi
{
    using System.Threading.Tasks;

    using Bank.BankEntities;

    public interface IBankApi
    {
        Task<BankResponse> Do(BankTransaction bankTransaction);

        Task<BankResponse> DoWithVisa(BankVisaTransaction bankTransaction);
    }
}