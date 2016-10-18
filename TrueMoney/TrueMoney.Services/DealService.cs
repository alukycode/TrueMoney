using TrueMoney.Infrastructure.Enums;

namespace TrueMoney.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TrueMoney.Infrastructure.Entities;
    using TrueMoney.Infrastructure.Services;

    public class DealService : IDealService
    {
        private readonly IUserService _userService;

        public DealService(IUserService userService)
        {
            _userService = userService;

            // review: по-хорошему, эти данные должны быть в репозитории, в методе-заглушке и возвращаться из него
            data = new List<Deal> // todo
                       {
                           new Deal
                               {
                                   Id = 0,
                                   Owner = new User { Id = 0 },//todo - get real user
                                   CreateDate = new DateTime(2016, 10, 09),
                                   InterestRate = 25,
                                   Description = "for business",
                               },
                           new Deal
                               {
                                   Id = 1,
                                   Owner = new User { Id = 1 },//todo - get real user
                                   CreateDate = new DateTime(2016, 10, 09),
                                   InterestRate = 25,
                                   Description = "to buy keyboard",
                                   Offers = new List<Offer>
                                                {
                                                    new Offer
                                                        {
                                                            Id = 0,
                                                            Offerer = new User { Id = 2 },
                                                            CreateTime = new DateTime(2016,10,09),
                                                            InterestRate = 20
                                                        },
                                                    new Offer
                                                        {
                                                            Id = 1,
                                                            Offerer = new User { Id = 0 },
                                                            CreateTime = new DateTime(2016,10,09),
                                                            InterestRate = 21
                                                        }
                                                },
                               },
                           new Deal
                               {
                                   Id = 2,
                                   Owner = new User { Id = 2 },//todo - get real user
                                   CreateDate = new DateTime(2016, 10, 09),
                                   InterestRate = 5,
                                   Description = "to rent a bitches",
                               }
                       };
        }

        static List<Deal> data;

        private static int number = 3;

        public async Task<IList<Deal>> GetAllOpen()
        {
            return data.Where(x => x.Status != DealStatus.Closed).ToList();
        }

        public async Task<IList<Deal>> GetAll()
        {
            return data;
        }

        public async Task<Deal> GetById(int id)
        {
            return data.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IList<Deal>> GetAllByUser(User user)
        {
            return data.Where(x => Equals(x.Owner, user)).ToList();
        }

        public async Task<Deal> GetByOfferId(int offerId)
        {
            return data.FirstOrDefault(x => x.Offers.Any(y => y.Id == offerId));
        }

        public async Task<IList<Offer>> GetAllOffersByUser(User user)
        {
            var res = new List<Offer>();
            foreach (var deal in data)
            {
                res.AddRange(deal.Offers.Where(x => x.Offerer.Id == user.Id));
            }

            return res;
        }

        public async Task ApplyOffer(int offerId, int dealId)
        {
            var deal = data.First(x => x.Id == dealId);
            var offer = deal.Offers.First(x => x.Id == offerId);
            //deal.WaitForApprove = true; меняй тут state
        }

        public async Task RevertOffer(int offerId)
        {
            //Тут должен юзаться OfferRepository, чтобы просто получить сущность по id и просто удалить ее, все!
            //var offer = deal?.Offers.FirstOrDefault(x => x.Id == offerId && Equals(x.Offerer, user));
            //if (offer != null)
            //{
            //    if (offer.WaitForApprove)
            //    {
            //        deal.WaitForApprove = false;
            //    }
            //    deal.Offers = deal.Offers.Where(x => x.Id != offerId).ToList();//del offer

            //    return true;
            //}

            //return false;
        }

        public async Task CreateOffer(User user, int dealId, int rate)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId);
            deal.Offers.Add(
                new Offer
                {
                    Id = number++,
                    CreateTime = DateTime.Now,
                    Offerer = user,
                    Deal = deal,
                    InterestRate = rate
                });
        }

        public async Task<Deal> FinishDealStartLoan(User user, int offerId, int dealId)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId);
            var finishOffer = deal.Offers.First(x => x.Id == offerId);
            deal.Status = DealStatus.WaitForLoan;
            deal.CloseDate = DateTime.Now; //это же должна быть тата, когда полностью закрыли всю сделку
            deal.InterestRate = finishOffer.InterestRate;
            //finish offer
            finishOffer.IsApproved = true;

            return deal;
        }

        public async Task<int> CreateDeal(User user, decimal count, int rate, string description) //Должен быть метод Add(Deal deal) и все, сама сущность будет мапиться из контроллера на новую, которую ты и будешь создавать
        {
            if (user.IsActive && !user.IsHaveOpenDealOrLoan)
            {
                data.Add(
                    new Deal
                    {
                        Id = number++,
                        Owner = user,
                        CreateDate = DateTime.Now,
                        Amount = count,
                        Description = description,
                        InterestRate = rate,
                    });
                user.IsHaveOpenDealOrLoan = true;

                return data[number - 1].Id;
            }

            return -1;
        }

        public async Task DeleteDeal(int dealId)
        {
            //тут будет просто await _dealRepository.Delete(dealId);
        }

        public async Task<Deal> PaymentFinished(Deal deal)
        {
            deal.Status = DealStatus.InProgress;
            deal.PaymentPlan = CalculatePayments(deal);

            return deal;
        }

        private PaymentPlan CalculatePayments(Deal deal)
        {
            return new PaymentPlan { Deal = deal, CreateTime = DateTime.Now };
        }
    }
}