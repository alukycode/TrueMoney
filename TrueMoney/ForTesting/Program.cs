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
                //using (var context = new TrueMoneyContext())
                //{
                //    var plan = context.PaymentPlans.Create();
                //    plan.CreateTime = DateTime.Now;

                //    var deal = context.Deals.Create();
                //    deal.OwnerId = 1;
                //    deal.CreateDate = DateTime.Now;

                //    deal.PaymentPlan = plan;

                //    context.Deals.Add(deal);
                //    context.SaveChanges();
                //}
                using (var context = new TrueMoneyContext())
                {
                    var deal = context.Deals.First(x => x.Id == 10);
                    context.Deals.Remove(deal);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
