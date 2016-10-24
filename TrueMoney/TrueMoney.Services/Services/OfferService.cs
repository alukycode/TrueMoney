using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Services.Services
{
    public class OfferService : IOfferService
    {
        private readonly ITrueMoneyContext _context;

        public OfferService(ITrueMoneyContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        public async Task<IList<OfferModel>> GetByUser(int userId)
        {
            var offers = await _context.Offers.Where(x => x.OffererId == userId).ToListAsync();
            return Mapper.Map<IList<OfferModel>>(offers);
        }
    }
}