namespace TrueMoney.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TrueMoney.Infrastructure.Entities;
    using TrueMoney.Infrastructure.Repositories;
    using TrueMoney.Infrastructure.Services;

    public class ApplicationService : IApplicationService
    {
        private readonly IUserService _userService;
        private readonly ILoanService _loanService;

        public ApplicationService(IUserService userService, ILoanService loanService)
        {
            _userService = userService;
            this._loanService = loanService;
            data = new List<MoneyApplication> // todo
                       {
                           new MoneyApplication
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
                           new MoneyApplication
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
                           new MoneyApplication
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

        static List<MoneyApplication> data;

        private static int number = 3;

        async Task<IList<MoneyApplication>> IApplicationService.GetAll()
        {
            return data.Where(x => !x.IsClosed).ToList();
        }

        async Task<MoneyApplication> IApplicationService.GetById(int id)
        {
            return data.FirstOrDefault(x => x.Id == id);
        }

        async Task<IList<MoneyApplication>> IApplicationService.GetByUserId(int userId)
        {
            return data.Where(x => x.Borrower.Id == userId).ToList();
        }

        async Task<MoneyApplication> IApplicationService.GetByOfferId(int offerId)
        {
            return data.FirstOrDefault(x => x.Offers.Any(y => y.Id == offerId));
        }

        public async Task<IList<Offer>> GetAllOffersByUser(int userId)
        {
            var res = new List<Offer>();
            foreach (var moneyApplication in data)
            {
                res.AddRange(moneyApplication.Offers.Where(x => x.Lender.Id == userId));
            }

            return res;
        }

        public async Task<bool> ApplyOffer(int offerId, int moneyApplicationId)
        {
            var app = data.FirstOrDefault(x => x.Id == moneyApplicationId);
            if (app != null)
            {
                var offer = app.Offers.FirstOrDefault(x => x.Id == offerId);
                if (offer != null)
                {
                    offer.WaitForApprove = true;
                    app.WaitForApprove = true;

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> RevertOffer(int offerId, int moneyApplicationId)
        {
            var app = data.FirstOrDefault(x => x.Id == moneyApplicationId);
            var offer = app?.Offers.FirstOrDefault(x => x.Id == offerId);
            if (offer != null)
            {
                if (offer.WaitForApprove)
                {
                    app.WaitForApprove = false;
                }
                app.Offers = app.Offers.Where(x => x.Id != offerId).ToList();//del offer

                return true;
            }

            return false;
        }

        public async Task<bool> CreateOffer(int appId, float rate)
        {
            var activeAppsByPerson = new List<MoneyApplication>();//todo - Sania - get all active apps by user;
            var app = data.FirstOrDefault(x => x.Id == appId);
            var user = await this._userService.GetCurrentUser();
            if (!activeAppsByPerson.Any() && user != null && user.IsActive && app != null && !app.IsClosed && 
                !app.Offers.Any(x=>!x.IsClosed && x.Lender.Id == user.Id))
            {
                app.Offers.Add(
                    new Offer
                        {
                            Id = number++,
                            CreateTime = DateTime.Now,
                            Lender = user,
                            MoneyApplication = app,
                            Rate = rate
                        });

                return true;
            }

            return false;
        }

        public async Task<int> FinishApp(int offerId, int moneyApplicationId)
        {
            var finishApp = data.FirstOrDefault(x => x.Id == moneyApplicationId);
            if (finishApp != null)
            {
                var finishOffer = finishApp.Offers.FirstOrDefault(x => x.Id == offerId);
                if (finishOffer != null)
                {
                    var newLoan = await this._loanService.Create(finishApp, finishOffer);
                    if (newLoan != null)
                    {
                        //finish app
                        finishApp.IsClosed = true;
                        finishApp.CloseDate = DateTime.Now;
                        finishApp.FinishOfferId = finishOffer.Id;
                        finishApp.FinishLoadId = newLoan.Id;

                        //finish offer
                        finishOffer.IsClosed = true;
                        finishOffer.CloseDate = DateTime.Now;

                        return newLoan.Id;
                    }
                }
            }

            return -1;
        }

        public async Task<int> CreateApp(float count, int paymentCount, float rate, int dayCount, string description)
        {
            var user = await this._userService.GetCurrentUser();
            if (user.IsActive && !user.IsHaveOpenAppOrLoan)
            {
                data.Add(
                    new MoneyApplication
                    {
                        Id = number++,
                        Borrower = await this._userService.GetCurrentUser(),
                        CreateDate = DateTime.Now,
                        Count = count,
                        Description = description,
                        Rate = rate,
                        DayCount = dayCount,
                        PaymentCount = paymentCount
                    });
                user.IsHaveOpenAppOrLoan = true;

                return data[number - 1].Id;
            }

            return -1;
        }

        public async Task<bool> DeleteApp(int appId)
        {
            var currentUser = await this._userService.GetCurrentUser();
            var app = data.FirstOrDefault(x => x.Id == appId);
            if (app != null && !app.IsClosed && app.IsTakePart(currentUser))
            {
                app.IsClosed = true;
                app.CloseDate = DateTime.Now;
                foreach (var offer in app.Offers.Where(x => !x.IsClosed))
                {
                    offer.IsClosed = true;
                    offer.CloseDate = DateTime.Now;
                    //send notification for lender
                }

                currentUser.IsHaveOpenAppOrLoan = false;

                return true;
            }

            return false;
        }
    }
}