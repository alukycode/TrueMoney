namespace TrueMoney.Services.Rating
{
    using TrueMoney.Infrastructure.Entities;

    public static class RatingHelper // review: почему нужен хелпер, если можно сделать метод в сервисе?
    {
        public static void ChangeRating(User user, RatingAction action)
        {
            switch (action)
            {
                case RatingAction.PayInTime:
                    user.Rating += 1;
                    break;
                case RatingAction.FinishLoan:
                    user.Rating += 10;
                    break;
                case RatingAction.LendMoney:
                    user.Rating += 5;
                    break;
                case RatingAction.MissPayTime:
                    user.Rating -= 2;
                    break;
            }
        }
    }
}