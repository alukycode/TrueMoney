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
    using TrueMoney.Models.User;
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

            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            _context = context;
            _userService = userService;
            _offerService = offerService;
        }

        public async Task<DealIndexViewModel> GetAllOpen(int currentUserId) //Пример адекватного метода
        {
            return new DealIndexViewModel()
            {
                CurrentUserId = currentUserId,
                Deals = Mapper.Map<List<DealModel>>(await _context.Deals.Where(x => x.DealStatus == DealStatus.Open).ToListAsync())
            };
        }

        public async Task<YourActivityViewModel> GetYourActivityViewModel(int currentUserId)
        {
            var deals = await GetByUser(currentUserId);
            var offers = await _offerService.GetByUser(currentUserId);
            var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);
            var model = new YourActivityViewModel
            {
                Deals = deals,
                Offers = offers,
                IsCurrentUserActive = user.IsActive
            };

            return model;
        }

        public async Task<DealIndexViewModel> GetAll(int userId)
        {
            return new DealIndexViewModel
                       {
                           Deals = Mapper.Map<IList<DealModel>>(await _context.Deals.ToListAsync()),
                           CurrentUserId = userId
                       };
        }

        public async Task<IList<DealModel>> GetByUser(int userId)
        {
            var deals = await _context.Deals.Where(x => x.OwnerId == userId).ToListAsync();
            return Mapper.Map<List<DealModel>>(deals);
        }

        public async Task<DealDetailsViewModel> GetById(int id, int userId)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == id);
            var result = new DealDetailsViewModel
                             {
                                 CurrentUserId = userId,
                                 Offers = Mapper.Map<IList<OfferModel>>(deal.Offers),
                                 Deal = Mapper.Map<DealModel>(deal),
                                 PaymentPlanModel = Mapper.Map<PaymentPlanModel>(deal.PaymentPlan)
                             };
            if (deal.PaymentPlan != null)
            {
                result.Payments = Mapper.Map<IList<PaymentModel>>(deal.PaymentPlan.Payments);
            }

            return result;
        }

        //public async Task<Deal> GetByOfferId(int offerId)
        //{
        //    var deals = await _context.Deals
        //        .Where(x => x.Offers.Any(y => y.OffererId == offerId))
        //        .ToListAsync();

        //    return Mapper.Map(deals); // а че за метод? он же вообще не юзается, пока коменчу
        //}

        public async Task ApproveOffer(int offerId)
        {
            var offer = await _context.Offers.FirstAsync(x => x.Id == offerId);
            offer.Deal.DealStatus = DealStatus.WaitForApprove;
            offer.IsApproved = true;
            await _context.SaveChangesAsync();
        }

        public async Task RevertOffer(int offerId)
        {
            var offer = await _context.Offers.FirstAsync(x => x.Id == offerId);
            offer.Deal.DealStatus = DealStatus.Open;
            offer.IsApproved = false;
            await _context.SaveChangesAsync();
        }

        public async Task<CreateDealForm> GetCreateDealForm(int userId)
        {
            var res = new CreateDealForm();
            var dealsByUser = await GetByUser(userId);
            if (dealsByUser.All(x => x.DealStatus == DealStatus.Closed))
            {
                res.IsUserCanCreateDeal = true; 
            }                                   

            return res;
        }

        public async Task<CreateOfferForm> GetCreateOfferForm(int dealId, int userId)
        {
            var deal = await _context.Deals.FirstAsync(x => x.Id == dealId);
            return new CreateOfferForm
            {
                DealRate = deal.InterestRate,
                DealId = dealId,
                IsUserCanCreateOffer =
                               deal.OwnerId != userId && deal.Offers.All(x => x.OffererId != userId) //какая-то сомнительная штука, надо будет перепроверить
            };
        }

        public async Task CreateOffer(CreateOfferForm model, int userId)
        {
            _context.Offers.Add(new Offer
            {
                CreateTime = DateTime.Now,
                OffererId = model.OffererId,
                DealId = model.DealId,
                InterestRate = model.InterestRate
            }); // тут нужен маппинг, но сейчас лень.

            await _context.SaveChangesAsync();
        }

        public async Task<DealModel> FinishDealStartLoan(int userId, int offerId, int dealId) //TODO: отрефакторить по аналогии с предыдущими
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
            var finishOffer = deal.Offers.First(x => x.Id == offerId);
            deal.DealStatus = DealStatus.WaitForLoan;
            deal.InterestRate = finishOffer.InterestRate;
            //finish offer
            finishOffer.IsApproved = true;

            return (await GetById(deal.Id, userId)).Deal;//todo - return deal model from db
        }

        public async Task<int> CreateDeal(CreateDealForm model, int userId)//TODO: отрефакторить по аналогии с предыдущими
        {
            var deal = new Deal
            {
                OwnerId = model.OwnerId,
                CreateDate = DateTime.Now,
                Amount = model.Amount,
                Description = model.Description,
                InterestRate = model.Rate,
                PaymentCount = model.PaymentCount

            };
            _context.Deals.Add(deal);
            // todo: commented after changing project structure -- user.IsHaveOpenDealOrLoan = true;
            await _context.SaveChangesAsync();
            return deal.Id; // надо поверить, что работает
        }

        public async Task DeleteDeal(int dealId, int userId)//TODO: отрефакторить по аналогии с предыдущими
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