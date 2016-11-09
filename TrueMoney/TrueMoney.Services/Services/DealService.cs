using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Data;
    using Interfaces;
    using Models.Offer;
    using TrueMoney.Models;
    using TrueMoney.Models.Deal;
    using TrueMoney.Models.User;

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

        public async Task<DealIndexViewModel> GetAll(int currentUserId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            return new DealIndexViewModel
            {
                Deals = Mapper.Map<IList<DealModel>>(await _context.Deals.ToListAsync()),
                UserCanCreateDeal = 
                    currentUser != null 
                    && currentUser.IsActive 
                    && currentUser.Deals.All(x => x.DealStatus == DealStatus.Closed)
            };
        }

        public async Task<IList<DealModel>> GetByUser(int userId)
        {
            var deals = await _context.Deals.Where(x => x.OwnerId == userId).ToListAsync();
            return Mapper.Map<List<DealModel>>(deals);
        }

        public async Task<DealDetailsViewModel> GetById(int id, int userId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var deal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == id);
            var result = new DealDetailsViewModel
            {
                CurrentUserId = userId,
                IsCurrentUserActive = currentUser.IsActive,
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

        public async Task FinishDealStartLoan(int offerId) 
        {
            var offer = await _context.Offers.FirstAsync(x => x.Id == offerId);
            var deal = offer.Deal;
            deal.DealStatus = DealStatus.WaitForLoan;
            deal.InterestRate = offer.InterestRate;
            offer.IsApproved = true;

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateDeal(CreateDealForm model, int userId)
        {
            var deal = Mapper.Map<Deal>(model);
            deal.OwnerId = userId;
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();
            return deal.Id;
        }

        public async Task DeleteDeal(int dealId, int userId)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
            _context.Deals.Remove(deal);

            await _context.SaveChangesAsync();
        }

        private PaymentPlan CalculatePayments(Deal deal)
        {
            return new PaymentPlan { Deal = deal, CreateTime = DateTime.Now };
        }
    }
}