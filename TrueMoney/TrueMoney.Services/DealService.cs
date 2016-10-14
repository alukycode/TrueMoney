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
        private readonly ILoanService _loanService;

        public DealService(IUserService userService, ILoanService loanService)
        {
            _userService = userService;
            this._loanService = loanService;

            // review: по-хорошему, эти данные должны быть в репозитории, в методе-заглушке и возвращаться из него
            data = new List<Deal> // todo
                       {
                           new Deal
                               {
                                   Id = 0,
                                   IsClosed = false,
                                   Borrower = new User { Id = 0 },//todo - get real user
                                   CreateDate = new DateTime(2016, 10, 09),
                                   Count = 100,
                                   Rate = 25f,
                                   Description = "for business",
                                   DayCount = 10
                               },
                           new Deal
                               {
                                   Id = 1,
                                   IsClosed = false,
                                   Borrower = new User { Id = 1 },//todo - get real user
                                   CreateDate = new DateTime(2016, 10, 09),
                                   Count = 200,
                                   Rate = 25f,
                                   Description = "to buy keyboard",
                                   Offers = new List<Offer>
                                                {
                                                    new Offer
                                                        {
                                                            Id = 0,
                                                            Lender = new User { Id = 2 },
                                                            CreateTime = new DateTime(2016,10,09),
                                                            Rate = 20f
                                                        },
                                                    new Offer
                                                        {
                                                            Id = 1,
                                                            Lender = new User { Id = 0 },
                                                            CreateTime = new DateTime(2016,10,09),
                                                            Rate = 21f
                                                        }
                                                },
                                   DayCount = 10
                               },
                           new Deal
                               {
                                   Id = 2,
                                   IsClosed = false,
                                   Borrower = new User { Id = 2 },//todo - get real user
                                   CreateDate = new DateTime(2016, 10, 09),
                                   Count = 2000,
                                   Rate = 5f,
                                   Description = "to rent a bitches",
                                   DayCount = 10
                               }
                       };
        }

        static List<Deal> data;

        private static int number = 3;

        public async Task<IList<Deal>> GetAll()
        {
            return data.Where(x => !x.IsClosed).ToList();
        }

        public async Task<Deal> GetById(int id)
        {
            return data.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IList<Deal>> GetAllByUser(User user)
        {
            return data.Where(x => Equals(x.Borrower, user)).ToList();
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
                res.AddRange(deal.Offers.Where(x => x.Lender.Id == user.Id));
            }

            return res;
        }

        public async Task<bool> ApplyOffer(User user, int offerId, int dealId)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId && Equals(x.Borrower, user));
            var offer = deal?.Offers.FirstOrDefault(x => x.Id == offerId);
            if (offer != null)
            {
                offer.WaitForApprove = true;
                deal.WaitForApprove = true;

                return true;
            }

            return false;
        }

        public async Task<bool> RevertOffer(User user, int offerId, int dealId)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId);
            var offer = deal?.Offers.FirstOrDefault(x => x.Id == offerId && Equals(x.Lender, user));
            if (offer != null)
            {
                if (offer.WaitForApprove)
                {
                    deal.WaitForApprove = false;
                }
                deal.Offers = deal.Offers.Where(x => x.Id != offerId).ToList();//del offer

                return true;
            }

            return false;
        }

        public async Task<bool> CreateOffer(User user, int dealId, float rate)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId);
            if (!user.IsHaveOpenDealOrLoan && user.IsActive && deal != null && !deal.IsClosed && 
                !deal.Offers.Any(x=>!x.IsClosed && Equals(x.Lender, user))) // review: должна быть весомая причина чтобы проверять юзера на null, ведь такого быть не должно
            {
                deal.Offers.Add(
                    new Offer
                        {
                            Id = number++,
                            CreateTime = DateTime.Now,
                            Lender = user,
                            Deal = deal,
                            Rate = rate
                        });

                return true;
            }

            return false; // review: не нужно этих true/false, не получилось сделать по каким-то причинам - кидай эксепшен
        }

        public async Task<int> FinishDeal(User user, int offerId, int dealId)
        {
            var finishDeal = data.FirstOrDefault(x => x.Id == dealId);
            var finishOffer = finishDeal?.Offers.FirstOrDefault(x => x.Id == offerId && Equals(x.Lender, user));
            if (finishOffer != null)
            {
                var newLoan = await this._loanService.Create(user, finishDeal, finishOffer);
                if (newLoan != null)
                {
                    //finish deal
                    finishDeal.IsClosed = true;
                    finishDeal.CloseDate = DateTime.Now;
                    finishDeal.FinishOfferId = finishOffer.Id;
                    finishDeal.FinishLoadId = newLoan.Id;

                    //finish offer
                    finishOffer.IsClosed = true;
                    finishOffer.CloseDate = DateTime.Now;

                    return newLoan.Id;
                }
            }

            return -1; // review: что это блять за магические цифры
        }

        public async Task<int> CreateDeal(User user, float count, int paymentCount, float rate, int dayCount, string description)
        {
            if (user.IsActive && !user.IsHaveOpenDealOrLoan)
            {
                data.Add(
                    new Deal
                    {
                        Id = number++,
                        Borrower = user,
                        CreateDate = DateTime.Now,
                        Count = count,
                        Description = description,
                        Rate = rate,
                        DayCount = dayCount,
                        PaymentCount = paymentCount
                    });
                user.IsHaveOpenDealOrLoan = true;

                return data[number - 1].Id;
            }

            return -1;
        }

        public async Task<bool> DeleteDeal(User currentUser, int dealIp)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealIp);
            if (deal != null && !deal.IsClosed && deal.IsTakePart(currentUser))
            {
                deal.IsClosed = true;
                deal.CloseDate = DateTime.Now;
                foreach (var offer in deal.Offers.Where(x => !x.IsClosed))
                {
                    offer.IsClosed = true;
                    offer.CloseDate = DateTime.Now;
                    //send notification for lender
                }

                currentUser.IsHaveOpenDealOrLoan = false; // todo: не вижу, что это где-то сохраняется

                return true;
            }

            return false;
        }
    }
}