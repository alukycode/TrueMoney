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
    using TrueMoney.Models.Admin;
    using TrueMoney.Models.Deal;
    using TrueMoney.Services.Extensions;

    public class DealService : IDealService
    {
        private readonly ITrueMoneyContext _context;

        public DealService(ITrueMoneyContext context)
        {
            _context = context;
        }

        public async Task<DealIndexViewModel> GetAll(int currentUserId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);

            var deals = await _context.Deals.Include(x => x.Offers).ToListAsync();

            var dealIdsWithOffers = new HashSet<int>();

            foreach (var deal in deals)
            {
                if (deal.Offers.Any(x => x.OffererId == currentUserId))
                {
                    dealIdsWithOffers.Add(deal.Id);
                }
            }

            return new DealIndexViewModel
            {
                Deals = Mapper.Map<IList<DealModel>>(deals).OrderByDescending(x => x.CreateDate).ToList(),
                UserCanCreateDeal =
                    currentUser != null
                    && currentUser.IsActive
                    && currentUser.Deals.All(x => x.DealStatus == DealStatus.Closed),
                DealIdsWithOfferFromCurrentUser = dealIdsWithOffers
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
            var haveOpenDeal = currentUser.Deals.Any(x => x.DealStatus != DealStatus.Closed);

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

            if (offer.OffererId != currentUserId)
            {
                throw new AccessViolationException("User try to finish deal and start loan for foreign deal.");
            }

            if (deal.DealStatus != DealStatus.WaitForApprove)
            {
                throw new AccessViolationException("User try to finish deal and start loan for deal with wrong status.");
            }

            deal.DealStatus = DealStatus.WaitForLoan;
            deal.InterestRate = offer.InterestRate;

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateDeal(int userId, CreateDealForm model)
        {
            var currentUser = await _context.Users.FirstAsync(x => x.Id == userId);
            var haveOpenDeal = currentUser.Deals.Any(x => x.DealStatus != DealStatus.Closed);

            if (haveOpenDeal)
            {
                throw new AccessViolationException("User try to create the 2nd open deal.");
            }

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