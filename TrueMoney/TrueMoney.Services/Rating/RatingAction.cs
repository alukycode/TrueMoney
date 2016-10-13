namespace TrueMoney.Services.Rating
{
    public enum RatingAction // review: enum явно надо размещать в проекте вроде Infrastructure, который виден всем
    {
        LendMoney, PayInTime, MissPayTime, FinishLoan
    }
}