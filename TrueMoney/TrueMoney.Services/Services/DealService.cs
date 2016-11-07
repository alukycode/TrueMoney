﻿using TrueMoney.Common.Enums;
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

<<<<<<< HEAD
        public async Task<DealIndexViewModel> GetAllOpen(int currentUserId) //Пример адекватного метода
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            return new DealIndexViewModel()
            {
                CurrentUserId = currentUserId,
                Deals = Mapper.Map<List<DealModel>>(await _context.Deals.Where(x => x.DealStatus == DealStatus.Open).ToListAsync()),
                IsCurrentUserActive = currentUser.IsActive
            };
        }

        public async Task<DealIndexViewModel> GetAllForAnonymous()
        {
            return new DealIndexViewModel
            {
                Deals = Mapper.Map<IList<DealModel>>(await _context.Deals.ToListAsync()),
                CurrentUserId = -1,
                IsCurrentUserActive = false
            };
=======
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
>>>>>>> bd2e144a1035e78195aa3de57cd1003ee4c25aa9
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

        public async Task FinishDealStartLoan(int userId, int offerId, int dealId) //TODO: отрефакторить по аналогии с предыдущими
        {
            var deal = await _context.Deals
                .Include(x => x.Offers)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == dealId);
            var finishOffer = deal.Offers.First(x => x.Id == offerId);
            deal.DealStatus = DealStatus.WaitForLoan;
            deal.InterestRate = finishOffer.InterestRate;
            //finish offer
            finishOffer.IsApproved = true;

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