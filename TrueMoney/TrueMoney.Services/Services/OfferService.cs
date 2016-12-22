using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Common.Enums;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;
using TrueMoney.Models.Offer;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Services.Services
{
    using TrueMoney.Common;

    public class OfferService : IOfferService
    {
        private readonly ITrueMoneyContext _context;

        public OfferService(ITrueMoneyContext context)
        {
            _context = context;
        }

        public async Task<IList<OfferModel>> GetByUser(int userId)
        {
            var offers = await _context.Offers.Where(x => x.OffererId == userId).ToListAsync();
            return Mapper.Map<IList<OfferModel>>(offers);
        }

        public async Task ApproveOffer(int offerId, int currentUserId)
        {
            var offer = await _context.Offers.SingleAsync(x => x.Id == offerId);

            if (offer.Deal.OwnerId != currentUserId)
            {
                throw new AccessViolationException("User try to approve the 2nd offer for one deal.");
            }

            if (offer.Deal.Offers.Any(x => x.IsApproved))
            {
                throw new AccessViolationException("User try to approve offer for foreign deal.");
            }

            offer.IsApproved = true;
            var deal = offer.Deal;
            deal.DealStatus = DealStatus.WaitForApprove;
            await _context.SaveChangesAsync();
        }

        public async Task CancelOfferApproval(int offerId, int currentUserId)
        {
            var offer = await _context.Offers
                .Include(x => x.Deal)
                .Include(x => x.Deal.Owner)
                .FirstAsync(x => x.Id == offerId);
            if (offer == null || !offer.IsApproved)
            {
                throw new AccessViolationException("User try to cancel approve for not approved offer.");
            }
            if (offer.Deal.OwnerId != currentUserId)
            {
                throw new AccessViolationException("User try to cancel offer for foreign deal.");
            }
            if (offer.Deal.DealStatus == DealStatus.Closed 
                || offer.Deal.DealStatus == DealStatus.InProgress
                || offer.Deal.DealStatus == DealStatus.WaitForLoan)
            {
                throw new AccessViolationException("Wrong deal status.");
            }

            offer.IsApproved = false;
            var deal = offer.Deal;
            deal.DealStatus = DealStatus.Open;
            await _context.SaveChangesAsync();
        }

        public async Task RevertOffer(int dealId, int currentUserId)
        {
            var offer = await _context.Offers.FirstAsync(x => x.DealId == dealId && x.OffererId == currentUserId);
            var deal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
            if (offer.OffererId != currentUserId)
            {
                throw new AccessViolationException("User try to delete foreign offer.");
            }

            if (offer.IsApproved)
            {
                if (deal.DealStatus == DealStatus.WaitForApprove)
                {
                    var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);
                    user.Rating += Rating.RevertFinalOffer;
                }
                deal.DealStatus = DealStatus.Open;
            }

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
        }

        public async Task<CreateOfferForm> GetCreateOfferForm(int dealId, int currentUserId)
        {
            var deal = await _context.Deals.FirstAsync(x => x.Id == dealId);
            var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);
            if (!user.IsActive)
            {
                throw new AccessViolationException("User can't create offer until he is active.");
            }

            return new CreateOfferForm
            {
                DealRate = deal.InterestRate,
                DealId = dealId,
                OffererId = currentUserId
            };
        }

        public async Task CreateOffer(CreateOfferForm model, int currentUserId)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == model.DealId);
            if (deal == null || deal.OwnerId == currentUserId ||
                deal.Offers.Any(x => x.OffererId == currentUserId) || model.OffererId != currentUserId)
            {
                throw new AccessViolationException("Wrong data to create new offer.");
            }

            _context.Offers.Add(Mapper.Map<Offer>(model));
            await _context.SaveChangesAsync();
        }
    }
}