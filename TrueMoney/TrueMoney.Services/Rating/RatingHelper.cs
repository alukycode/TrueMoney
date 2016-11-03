using TrueMoney.Data.Entities;

namespace TrueMoney.Services.Rating
{

    internal static class RatingHelper // review: почему нужен хелпер, если можно сделать метод в сервисе?
    {
        public static void ChangeRating(User user, RatingAction action)
        {
            switch (action)
            {
                case RatingAction.PayInTime:
                    user.Rating += 1; // review: magic numbers, вынести в файл NumericConstants.cs в проектe Common
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