
using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Data;
    using Interfaces;
    using TrueMoney.Models;
    using TrueMoney.Models.ViewModels;

    public class DealService : IDealService
    {
        private readonly IUserService _userService;
        private readonly IOfferService _offerService;
        private readonly ITrueMoneyContext _context;

        public DealService(
            IUserService userService,
            ITrueMoneyContext context,
            IOfferService offerService)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (offerService == null)
            {
                throw new ArgumentNullException(nameof(offerService));
            }

            _context = context;
            _userService = userService;
            _offerService = offerService;

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

        public async Task<DealIndexViewModel> GetAllOpen(int currentUserId) //Пример адекватного метода
        {
            var openDeals = await _context.Deals.Where(x => x.DealStatus == DealStatus.Open).ToListAsync();
            var result = new DealIndexViewModel()
            {
                CurrentUserId = currentUserId,
                Deals = Mapper.Map<List<DealModel>>(openDeals)
            };

            return result;
        }

        public async Task<YourActivityViewModel> GetYourActivityViewModel(int currentUserId) // проверять, что текущий юзер вообще может вызвать этот метод надо бы на контроллере
        {
            var deals = await GetByUser(currentUserId); 
            var offers = await _offerService.GetByUser(currentUserId);
            var model = new YourActivityViewModel
            {
                Deals = deals, 
                Offers = offers 
            };

            return model;
        }

        public async Task<IList<DealIndexViewModel>> GetAll(int userId)
        {
            return Mapper.Map<IList<DealIndexViewModel>>(data, opt => opt.Items["currentUserId"] = userId);
        }

        public async Task<IList<DealModel>> GetByUser(int userId) // еще один пример нормального метода
        {
            var deals = await _context.Deals.Where(x => x.OwnerId == userId).ToListAsync();
            return Mapper.Map<List<DealModel>>(deals);
        }

        public async Task<DealDetailsViewModel> GetById(int id, int userId)
        {
            var deal = data.FirstOrDefault(x => x.Id == id);

            return Mapper.Map<DealDetailsViewModel>(deal, opt => opt.Items["currentUserId"] = userId);
        }

        public async Task<Deal> GetByOfferId(int offerId)
        {
            return data.FirstOrDefault(x => x.Offers.Any(y => y.Id == offerId));
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
            var dealsByUser = await GetByUser(userId);
            if (dealsByUser.All(x => x.DealStatus == DealStatus.Closed))
            {
                res.IsUserCanCreateDeal = true; //какого хуя это вообще тут проверяется?? у него даже кнопки создать быть не должно
            }                                   //Димон: тут как раз это и првоерятеся, рисовать форму или нет

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
            deal.DealStatus = DealStatus.InProgress;

            return deal;
        }

        private PaymentPlan CalculatePayments(Deal deal)
        {
            return new PaymentPlan { Deal = deal, CreateTime = DateTime.Now };
        }
    }
}