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
            var users = Mapper.Map<IList<UserModel>>(await _context.Users.ToListAsync());
            var passports = Mapper.Map<IList<PassportModel>>(await _context.Passports.ToListAsync());

            return new InactiveUsersViewModel
                       {
                           Users =
                               users.Where(x => x.PassportId.HasValue)
                               .Select(
                                   x =>
                                   new UserAndPassportViewModel
                                       {
                                           User = x,
                                           Passport =
                                               passports.FirstOrDefault(
                                                   y =>
                                                   y.Id == x.PassportId.Value)
                                       })
                               .ToList()
                       };
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
            user.Rating = Rating.StartRating;
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
