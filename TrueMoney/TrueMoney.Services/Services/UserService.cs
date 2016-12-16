using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Models;
using TrueMoney.Models.Account;
using TrueMoney.Models.Admin;
using TrueMoney.Models.Basic;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Services.Services
{
    using Common.Enums;
    using TrueMoney.Common;
    using TrueMoney.Models.User;

    public class UserService : IUserService
    {
        private readonly ITrueMoneyContext _context;

        public UserService(ITrueMoneyContext context)
        {
            _context = context;
        }

        public async Task<UserDetailsViewModel> GetDetails(int userId)
        {
            var dbUser = await _context.Users.FirstAsync(x => x.Id == userId);

            var result = new UserDetailsViewModel
            {
                User = Mapper.Map<UserModel>(dbUser),
            };

            return result;
        }

        public async Task<UserModel> GetById(int id)
        {
            var dbUser = await _context.Users.FirstAsync(x => x.Id == id);
            var userModel = Mapper.Map<UserModel>(dbUser);

            return userModel;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            throw new NotImplementedException();
            //return await _userRepository.GetAll();
        }

        public User GetMappedUserEnity(RegisterViewModel model)
        {
            var user = Mapper.Map<User>(model);
            user.LockoutEnabled = true;
            user.UserName = model.Email;

            return user;
        }

        public async Task<InactiveUsersViewModel> GetInactiveUsersViewModel()
        {
            var users = await _context.Users.Where(x => x.PassportId != null).Include(x => x.Offers).Include(x => x.Deals).ToListAsync();
            var passports = Mapper.Map<IList<PassportModel>>(await _context.Passports.ToListAsync());

            var model = new InactiveUsersViewModel { Users = new List<UserAndPassportViewModel>() };

            foreach (var user in users)
            {
                var userAndPassportModel = new UserAndPassportViewModel
                {
                    IsUserCanBeDeactivated = user.IsActive && user.Deals.All(d => d.DealStatus == DealStatus.Open || d.DealStatus == DealStatus.Closed) && user.Offers.All(o => !o.IsApproved || o.Deal.DealStatus == DealStatus.Closed),
                    CountOfAllDeals = user.Deals.Count(d => d.DealStatus != DealStatus.Closed),
                    CountOfDealsInProgress = user.Deals.Count(d => d.DealStatus != DealStatus.Closed && d.DealStatus != DealStatus.Open),
                    CountOfAllOffers = user.Offers.Count(o => o.Deal.DealStatus != DealStatus.Closed),
                    CountOfApprovedOffers = user.Offers.Count(o => o.Deal.DealStatus != DealStatus.Closed && o.IsApproved),
                    User = Mapper.Map<UserModel>(user),
                    Passport = passports.First(y => y.Id == user.PassportId.Value)
                };

                model.Users.Add(userAndPassportModel);
            }

            return model;
        }

        //public async Task<UserActivityViewModel> GetProfileViewModel(int currentUserId)
        //{
        //    var offers = await _context.Offers.Where(x => x.OffererId == currentUserId).ToListAsync();
        //    var deals = await _context.Deals.Where(x => x.OwnerId == currentUserId).ToListAsync();
        //    var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);

        //    return new UserActivityViewModel
        //    {
        //        Deals = Mapper.Map<List<DealModel>>(deals),
        //        Offers = Mapper.Map<IList<OfferModel>>(offers),
        //        IsCurrentUserActive = user.IsActive
        //    };
        //}

        public async Task ActivateUser(int userId)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == userId);
            user.IsActive = true;
            user.Rating = Rating.AfterActivation;
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateUser(int userId)
        {
            var user = await _context.Users.Include(x => x.Offers).Include(x => x.Deals).FirstAsync(x => x.Id == userId);

            var isUserCanBeDeactivated =
                user.Deals.All(d => d.DealStatus == DealStatus.Open || d.DealStatus == DealStatus.Closed) &&
                user.Offers.All(o => !o.IsApproved || o.Deal.DealStatus == DealStatus.Closed);

            if (isUserCanBeDeactivated)
            {
                user.IsActive = false;
                if (user.Rating >= 0)
                {
                    user.Rating = Rating.AfterDeactivation;
                }

                foreach (var offer in user.Offers.ToList())
                {
                    _context.Offers.Remove(offer);
                }

                foreach (var deal in user.Deals.ToList())
                {
                    _context.Deals.Remove(deal);
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<EditUserViewModel> GetEditModel(int id)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == id);
            var model = Mapper.Map<EditUserViewModel>(user);

            return model;
        }

        public async Task<EditPassportViewModel> GetEditPassportModel(int userId)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == userId);
            var model = new EditPassportViewModel
            {
                Passport = Mapper.Map<PassportModel>(user.Passport),
            };

            return model;
        }

        public async Task<UserProfileModel> GetUserProfileModel(int userId)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == userId);
            var offers = user.Offers.Where(x => x.Deal.DealStatus == DealStatus.Open || x.IsApproved);

            var activeDeal = user.Deals.FirstOrDefault(x => x.DealStatus != DealStatus.Closed);
            var dealInfo = activeDeal == null ? null : new DealInfoModel
            {
                OffersCount = activeDeal.Offers.Count,
                BestOfferPercent = activeDeal.Offers.OrderBy(x => x.InterestRate).FirstOrDefault()?.InterestRate ?? 0
            };

            var model = new UserProfileModel
            {
                User = Mapper.Map<UserModel>(user),
                Passport = Mapper.Map<PassportModel>(user.Passport),
                Deals = Mapper.Map<List<DealModel>>(user.Deals),
                Offers = Mapper.Map<IList<OfferModel>>(offers),
                IsCurrentUserActive = user.IsActive,
                ActiveDealInfo = dealInfo
            };

            return model;
        }

        public async Task Update(EditUserViewModel model)
        {
            var user = _context.Users.First(x => x.Id == model.Id);
            Mapper.Map(model, user, typeof(EditUserViewModel), typeof(User));
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePassport(EditPassportViewModel model)
        {
            var passport = _context.Passports.First(x => x.Id == model.Passport.Id);
            Mapper.Map(model.Passport, passport, typeof(PassportModel), typeof(Passport));
            await _context.SaveChangesAsync();
        }

        public async Task<UserActivityViewModel> GetUserActivityModel(int userId)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == userId);
            var offers = user.Offers;
            var deals = user.Deals;

            return new UserActivityViewModel
            {
                Deals = Mapper.Map<List<DealModel>>(deals),
                Offers = Mapper.Map<IList<OfferModel>>(offers),
                IsCurrentUserActive = user.IsActive
            };
        }
    }
}
