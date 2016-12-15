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
    using TrueMoney.Models.Admin;
    using TrueMoney.Models.Deal;
    using TrueMoney.Models.User;
    using TrueMoney.Services.Extensions;

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
            _context = context;
            _userService = userService;
            _offerService = offerService;
        }

        public async Task<DealIndexViewModel> GetAll(int currentUserId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            return new DealIndexViewModel
            {
                Deals = Mapper.Map<IList<DealModel>>(await _context.Deals.ToListAsync()), //.OrderByDescending(x => x.CreateDate).ToList(),
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
            var deal = await _context.Deals
                .Include(x => x.Owner)
                .Include(x => x.PaymentPlan.Payments)
                .Include(x => x.PaymentPlan.BankTransactions).FirstOrDefaultAsync(x => x.Id == id);
            var result = new DealDetailsViewModel
            {
                CurrentUserId = userId,
                IsCurrentUserActive = currentUser.IsActive,
                DealOwner = Mapper.Map<UserModel>(deal.Owner),
                Offers = Mapper.Map<IList<OfferModel>>(deal.Offers),
                Deal = Mapper.Map<DealModel>(deal),
                PaymentPlanModel = Mapper.Map<PaymentPlanModel>(deal.PaymentPlan)
            };
            if (deal.PaymentPlan != null)
            {
                var allPaidBefore =
                    deal.PaymentPlan.Payments.Where(x => x.IsPaid).Select(x => x.Amount + x.Liability).Sum();
                //some extra money before previous payment
                result.ExtraMoney = deal.PaymentPlan.BankTransactions.Select(x => x.Amount).Sum() - allPaidBefore;
                result.Payments = Mapper.Map<IList<PaymentModel>>(deal.PaymentPlan.Payments.CalculateLiability(result.ExtraMoney, deal));
            }

            //save calculatrd liability
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<CreateDealForm> GetCreateDealForm(int currentUserId)
        {
            var currentUser = await _context.Users.FirstAsync(x => x.Id == currentUserId);
            var haveOpenDeal = currentUser.Deals.Any(x => x.DealStatus == DealStatus.Open);
            var res = new CreateDealForm
            {
                IsCurrentUserActive = currentUser.IsActive,
                HaveOpenDeal = haveOpenDeal
            };

            return res;
        }

        public async Task<DealListViewModel> GetDealListViewModel()
        {
            return new DealListViewModel
                       {
                           Deals = Mapper.Map<IList<DealModel>>(await _context.Deals.ToListAsync()),
                           Payments =
                               Mapper.Map<IList<PaymentModel>>(await _context.Payments.ToListAsync()),
                           PaymentPlans =
                               Mapper.Map<IList<PaymentPlanModel>>(await _context.PaymentPlans.ToListAsync()),
                           BankTransactions =
                               Mapper.Map<IList<BankTransactionModel>>(await _context.BankTransactions.ToListAsync()),
                           Offers =
                               Mapper.Map<IList<OfferModel>>(await _context.Offers.Where(x => x.IsApproved).ToListAsync())
                       };
        }

        public async Task FinishDealStartLoan(int dealId, int currentUserId) 
        {
            var deal = await _context.Deals.FirstAsync(x => x.Id == dealId);
            var offer = await _context.Offers.FirstAsync(x => x.IsApproved && x.DealId == dealId && x.OffererId == currentUserId);
            deal.DealStatus = DealStatus.WaitForLoan;
            deal.InterestRate = offer.InterestRate;

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateDeal(int userId, CreateDealForm model)
        {
            var deal = Mapper.Map<Deal>(model);
            deal.OwnerId = userId;
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();

            return deal.Id;
        }

        public async Task DeleteDeal(int dealId, int currentUserId)
        {
            var deal = await _context.Deals.FirstAsync(x => x.Id == dealId && x.OwnerId == currentUserId);
            _context.Deals.Remove(deal);

            await _context.SaveChangesAsync();
        }
    }
}