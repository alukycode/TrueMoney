using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Services.Mapping;

namespace ForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var context = new TrueMoneyContext())
                {
                    var deal = context.Deals.Create();
                    deal.OwnerId = 1;
                    deal.CreateDate = DateTime.Now;
                    deal.Description = "asasasa";

                    var offer = context.Offers.Create();
                    offer.OffererId = 1;
                    offer.CreateTime = DateTime.Now;
                    deal.Offers = new List<Offer>
                    {
                        offer
                    };

                    context.Deals.Add(deal);

                    context.SaveChanges();
                }
                //using (var context = new TrueMoneyContext())
                //{
                //    var deal = context.Deals.First(x => x.Id == 2);
                //    deal.ResultOffer = deal.Offers.First();
                //    context.SaveChanges();
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
