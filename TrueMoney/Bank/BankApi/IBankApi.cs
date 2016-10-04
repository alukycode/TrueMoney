namespace Bank.BankApi
{
    using Bank.BankEntities;

    public interface IBankApi
    {
        BankResponse Do(BankTransaction bankTransaction);
    }
}