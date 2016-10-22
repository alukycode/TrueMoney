
using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

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
                    BorrowerId = deal.OwnerId,
                    Amount = deal.Amount,
                    CreateDate = deal.CreateDate,
                    DayCount = deal.DealPeriod.Days,
                    Description = deal.Description,
                    IsInProgress = deal.DealStatus == DealStatus.InProgress,
                    IsOpen = deal.DealStatus == DealStatus.Open,
                    IsWaitForLoan = deal.DealStatus == DealStatus.WaitForLoan,
                    IsWaitForApprove = deal.DealStatus == DealStatus.WaitForApprove,
                    Offers = deal.Offers.Select(x => new OfferModel())
                };
                res.CurrentUserId = userId;
                // todo - map offer to offer details
                //res.CurrentUserOffer = deal.Offers.FirstOrDefault(x => x.OffererId == userId);
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
                res.AddRange(deal.Offers.Where(x => x.Offerer.Id == userId));
            }

            //return res;
            throw new NotImplementedException();
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

        public async Task<DealModel> FinishDealStartLoan(int userId, int offerId, int dealId)
        {
            var deal = data.FirstOrDefault(x => x.Id == dealId);
            var finishOffer = deal.Offers.First(x => x.Id == offerId);
            deal.DealStatus = DealStatus.WaitForLoan;
            deal.CloseDate = DateTime.Now; //это же должна быть тата, когда полностью закрыли всю сделку
            deal.InterestRate = finishOffer.InterestRate;
            //finish offer
            finishOffer.IsApproved = true;

            //return deal;

            throw new NotImplementedException();
        }

        public async Task<int> CreateDeal(User user, decimal count, int rate, string description) //Должен быть метод Add(Deal deal) и все, сама сущность будет мапиться из контроллера на новую, которую ты и будешь создавать
        {
            // todo: commented after changing project structure -- if (user.IsActive && !user.IsHaveOpenDealOrLoan)
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
                // todo: commented after changing project structure -- user.IsHaveOpenDealOrLoan = true;

                return data[number - 1].Id;
            }

            return -1;
        }

        public async Task DeleteDeal(int dealId)
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