
using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using TrueMoney.Models;
    using TrueMoney.Models.ViewModels;

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
                                   OwnerId = 0,
                                   CreateDate = new DateTime(2016, 10, 09),
                                   InterestRate = 25,
                                   Description = "for business",
                                   Amount = 100
                               },
                           new Deal
                               {
                                   Id = 1,
                                   Owner = new User { Id = 1 },//todo - get real user
                                   OwnerId = 1,
                                   Amount = 200,
                                   CreateDate = new DateTime(2016, 10, 09),
                                   InterestRate = 25,
                                   Description = "to buy keyboard",
                                   Offers = new List<Offer>
                                                {
                                                    new Offer
                                                        {
                                                            Id = 0,
                                                            Offerer = new User { Id = 2 },
                                                            OffererId = 2,
                                                            CreateTime = new DateTime(2016,10,09),
                                                            InterestRate = 20
                                                        },
                                                    new Offer
                                                        {
                                                            Id = 1,
                                                            Offerer = new User { Id = 0 },
                                                            OffererId = 0,
                                                            CreateTime = new DateTime(2016,10,09),
                                                            InterestRate = 21
                                                        }
                                                },
                               },
                           new Deal
                               {
                                   Id = 2,
                                   Owner = new User { Id = 2 },//todo - get real user
                                   OwnerId = 2,
                                   CreateDate = new DateTime(2016, 10, 09),
                                   InterestRate = 5,
                                   Description = "to rent a bitches",
                                   Amount = 300
                               }
                       };
        }

        static List<Deal> data;

        private static int number = 3; // что за нумбер блять - ид для новых энитити пока Саня не сделает базу

        public async Task<IList<DealIndexViewModel>> GetAllOpen(int userId)
        {
            return
                data.Where(x => x.DealStatus == DealStatus.Open)
                    .Select(
                        x =>
                        new DealIndexViewModel
                        {
                            Deal = new DealModel
                            {
                                Amount = x.Amount,
                                CreateDate = x.CreateDate,
                                Id = x.Id,
                                DayCount = x.DealPeriod.Days,
                                Description = x.Description,
                                Rate = x.InterestRate
                            },
                            IsCurrentUserOwner = x.OwnerId == userId
                        })
                    .ToList();
        }

        public async Task<IList<DealIndexViewModel>> GetAll(int userId)
        {
            return
                data.Select(
                        x =>
                        new DealIndexViewModel
                        {
                            Deal = new DealModel
                            {
                                Amount = x.Amount,
                                CreateDate = x.CreateDate,
                                Id = x.Id,
                                DayCount = x.DealPeriod.Days,
                                Description = x.Description,
                                Rate = x.InterestRate
                            },
                            IsCurrentUserOwner = x.OwnerId == userId
                        })
                    .ToList();
        }

        public async Task<IList<DealModel>> GetAllByUser(int userId)
        {
            return
                data.Where(x => x.OwnerId == userId)
                    .Select(x => new DealModel
                    {
                        Amount = x.Amount,
                        CreateDate = x.CreateDate,
                        Id = x.Id,
                        DayCount = x.DealPeriod.Days,
                        Description = x.Description,
                        Rate = x.InterestRate
                    })
                    .ToList();
        }

        public async Task<DealDetailsViewModel> GetById(int id, int userId)
        {
            var res = new DealDetailsViewModel();
            var deal = data.FirstOrDefault(x => x.Id == id);

            if (deal != null)
            {
                res.IsCurrentUserBorrower = deal.OwnerId == userId;
                res.IsCurrentUserLender = deal.Offers.Any(x => x.OffererId == userId);
                res.Deal = new DealModel
                {
                    BorrowerFullName = string.Concat(deal.Owner.FirstName, " ", deal.Owner.LastName),
                    Borrower = new UserModel { Id = deal.Owner.Id },//todo - get user model
                    Amount = deal.Amount,
                    CreateDate = deal.CreateDate,
                    DayCount = deal.DealPeriod.Days,
                    Description = deal.Description,
                    IsInProgress = deal.DealStatus == DealStatus.InProgress,
                    IsOpen = deal.DealStatus == DealStatus.Open,
                    IsWaitForLoan = deal.DealStatus == DealStatus.WaitForLoan,
                    IsWaitForApprove = deal.DealStatus == DealStatus.WaitForApprove,
                    Rate = deal.InterestRate,
                    Id = deal.Id
                };
                res.CurrentUserId = userId;
                res.Offers =
                    deal.Offers.Select(
                        x =>
                        new OfferModel
                            {
                                DealId = x.DealId,
                                Id = x.Id,
                                Rate = x.InterestRate,
                                IsCurrentUserLender = x.OffererId == userId,
                                IsApproved = x.IsApproved,
                                Lender = new UserModel { Id = x.Offerer.Id, FirstName = x.Offerer.FirstName }
                            });
                res.CurrentUserOffer = res.Offers?.FirstOrDefault(x => x.Lender.Id == userId);
            }

            return res;
        }

        public async Task<Deal> GetByOfferId(int offerId)
        {
            return data.FirstOrDefault(x => x.Offers.Any(y => y.Id == offerId));
        }

        public async Task<IList<OfferModel>> GetAllOffersByUser(int userId)
        {
            var res = new List<Offer>();
            foreach (var deal in data)
            {
                if (deal.Offers != null)
                {
                    res.AddRange(deal.Offers.Where(x => x.OffererId == userId));
                }
            }

            return res.Select(x => new OfferModel
            {
                DealId = x.DealId,
                Id = x.Id,
                Rate = x.InterestRate,
                IsCurrentUserLender = x.OffererId == userId,
                IsApproved = x.IsApproved
            }).ToList();
        }

        public async Task ApplyOffer(int offerId, int dealId)
        {
            var deal = data.First(x => x.Id == dealId);
            var offer = deal.Offers.First(x => x.Id == offerId);
            //deal.WaitForApprove = true; меняй тут state
        }

        public async Task RevertOffer(int offerId, int userId)
        {
            //Тут должен юзаться OfferRepository, чтобы просто получить сущность по id и просто удалить ее, все!
            throw new NotImplementedException();
        }

        public async Task<CreateDealForm> GetCreateDealForm(int userId)
        {
            var res = new CreateDealForm();
            var dealsByUser = await GetAllByUser(userId);
            if (dealsByUser.All(x => x.IsClosed))
            {
                res.IsUserCanCreateDeal = true;
            }

            return res;
        }

        public async Task<CreateOfferForm> GetCreateOfferForm(int dealId, int userId)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId);
            return new CreateOfferForm
            {
                DealRate = deal.InterestRate,
                DealId = dealId,
                IsUserCanCreateOffer =
                               deal.OwnerId != userId && deal.Offers.All(x => x.OffererId != userId)
            };
        }

        public async Task<DealDetailsViewModel> CreateOffer(CreateOfferForm createOfferForm, int userId)
        {
            //todo - update db model in db
            var deal = data.FirstOrDefault(x => x.Id == createOfferForm.DealId);
            deal.Offers.Add(
                new Offer
                {
                    Id = number++,
                    CreateTime = DateTime.Now,
                    Offerer = new User { Id = userId },
                    Deal = deal,
                    InterestRate = createOfferForm.Rate
                });

            return await GetById(deal.Id, userId);//todo - return deal model from db
        }

        public async Task<DealModel> FinishDealStartLoan(int userId, int offerId, int dealId)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId);
            var finishOffer = deal.Offers.First(x => x.Id == offerId);
            deal.DealStatus = DealStatus.WaitForLoan;
            deal.InterestRate = finishOffer.InterestRate;
            //finish offer
            finishOffer.IsApproved = true;

            return (await GetById(deal.Id, userId)).Deal;//todo - return deal model from db
        }

        public async Task<DealModel> CreateDeal(CreateDealForm createDealForm, int userId)
        {
            data.Add(
                    new Deal
                    {
                        Id = number++,
                        Owner = new User { Id = userId },
                        CreateDate = DateTime.Now,
                        Amount = createDealForm.Amount,
                        Description = createDealForm.Description,
                        InterestRate = createDealForm.Rate,
                        PaymentCount = createDealForm.PaymentCount

                    });
            // todo: commented after changing project structure -- user.IsHaveOpenDealOrLoan = true;

            return (await GetById(number - 1, userId)).Deal;//todo - return deal model from db
        }

        public async Task DeleteDeal(int dealId, int userId)
        {
            //тут будет просто await _dealRepository.Delete(dealId);
        }

        public async Task<DealModel> PaymentFinished(DealModel deal)
        {
            //deal.DealStatus = DealStatus.InProgress;
            //deal.PaymentPlan = CalculatePayments(deal);
            deal.IsInProgress = true;//change status to InProgress in db
            deal.IsWaitForLoan = false;

            return deal;
        }

        private PaymentPlan CalculatePayments(Deal deal)
        {
            return new PaymentPlan { Deal = deal, CreateTime = DateTime.Now };
        }
    }
}