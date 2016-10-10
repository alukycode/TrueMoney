﻿namespace TrueMoney.Services
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
        private readonly IUserRepository _userRepository;

        public ApplicationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                                   Description = "for business"
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
                                                }
                               },
                           new MoneyApplication
                               {
                                   Id = 2,
                                   IsClosed = false,
                                   Borrower = new User { Id = 2 },//todo - get real user
                                   CreateDate = new DateTime(2016, 10, 09),
                                   Count = 2000,
                                   Rate = 5f,
                                   Description = "to rent a bitches"
                               }
                       };
        }

        static List<MoneyApplication> data;

        async Task<IList<MoneyApplication>> IApplicationService.GetAll()
        {
            return data;
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
            if (app != null)
            {
                var offer = app.Offers.FirstOrDefault(x => x.Id == offerId);
                if (offer != null)
                {
                    app.WaitForApprove = false;
                    app.Offers = app.Offers.Where(x => x.Id != offerId);//del offer

                    return true;
                }
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
                    var newLoan = new Loan
                    {
                        Borrower = finishApp.Borrower,
                        CloseDate = DateTime.Now,
                        Id = 0,
                        Lender = finishOffer.Lender,
                        MoneyApplication = finishApp
                    };
                    finishApp.IsClosed = true;
                    finishApp.CloseDate = DateTime.Now;
                    finishApp.FinishOfferId = finishOffer.Id;
                    finishApp.FinishLoadId = newLoan.Id;
                    finishOffer.IsClosed = true;
                    finishOffer.CloseDate = DateTime.Now;

                    return newLoan.Id;
                }
            }

            return -1;
        }
    }
}