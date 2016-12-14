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

        public async Task ApproveOffer(int offerId)
        {
            var offer = await _context.Offers
                .Include(x => x.Deal)
                .Include(x => x.Deal.Owner)
                .FirstAsync(x => x.Id == offerId);
            offer.IsApproved = true;
            var deal = offer.Deal;
            deal.DealStatus = DealStatus.WaitForApprove;
            await _context.SaveChangesAsync();
        }

        public async Task CancelOfferApproval(int offerId)
        {
            var offer = await _context.Offers
                .Include(x => x.Deal)
                .Include(x => x.Deal.Owner)
                .FirstAsync(x => x.Id == offerId);
            offer.IsApproved = false;
            var deal = offer.Deal;
            deal.DealStatus = DealStatus.Open;
            await _context.SaveChangesAsync();
        }

        public async Task RevertOffer(int dealId, int currentUserId)
        {
            // todo: validate if currentUserId allowed to perform this action
            var offer = await _context.Offers.FirstAsync(x => x.DealId == dealId && x.OffererId == currentUserId);
            var deal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
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

        public async Task CreateOffer(CreateOfferForm model)
        {
            _context.Offers.Add(Mapper.Map<Offer>(model));

            await _context.SaveChangesAsync();
        }
    }
}