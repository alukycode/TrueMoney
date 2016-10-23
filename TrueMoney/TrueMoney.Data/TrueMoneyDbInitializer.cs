using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data
{
    public class TrueMoneyDbInitializer : DropCreateDatabaseIfModelChanges<TrueMoneyContext>
    {
        protected override void Seed(TrueMoneyContext context)
        {
            base.Seed(context);
            InitializeData(context);
        }

        public static void InitializeData(TrueMoneyContext context)
        {
            List<User> users = new List<User>
            {
                new User
                {
                    FirstName = "Саша",
                    LastName = "Черногребель",
                    AspUserId = "change it!",
                    BankAccountNumber = "test",
                    Passport = new Passport()
                    {
                        DateOfIssuing = DateTime.Now,
                        Number = "test",
                        Series = "test",
                    }
                },
                new User
                {
                    FirstName = "Антон",
                    LastName = "Лукьянов",
                    AspUserId = "change it!",
                    BankAccountNumber = "test",
                    Passport = new Passport()
                    {
                        DateOfIssuing = DateTime.Now,
                        Number = "test",
                        Series = "test",
                    }
                },
                new User
                {
                    FirstName = "Дима",
                    LastName = "Артюх",
                    AspUserId = "change it!",
                    BankAccountNumber = "test",
                }
            };

            var user = users.First();
            var offerer = users.ElementAt(1);
            user.Deals = GenerateDeals(offerer);
            user.Deals.First().PaymentPlan = GeneratePlan();

            foreach (var item in users)
            {
                context.Users.Add(item);
            }

            context.SaveChanges();
        }

        private static PaymentPlan GeneratePlan()
        {
            return new PaymentPlan()
            {
                CreateTime = DateTime.Now,
                Payments = new List<Payment>()
                {
                    new Payment()
                    {
                        Amount = 10,
                        DueDate = DateTime.Now,
                    },
                    new Payment()
                    {
                        Amount = 20,
                        DueDate = DateTime.Now,
                    }
                }
            };
        }

        private static List<Deal> GenerateDeals(User offerer)
        {
            var result = new List<Deal>()
            {
                new Deal()
                {
                    Amount = 13,
                    CreateDate = DateTime.Now,
                    DealPeriod = new TimeSpan(5000, 0, 0, 0),
                    InterestRate = 12,
                    DealStatus = DealStatus.InProgress,
                    Offers = new List<Offer>()
                    {
                        new Offer()
                        {
                            CreateTime = DateTime.Now,
                            InterestRate = 10,
                            IsApproved = true,
                            Offerer = offerer,
                        }
                    }
                },
                new Deal()
                {
                    Amount = 123,
                    CreateDate = DateTime.Now,
                    DealPeriod = new TimeSpan(60, 0, 0, 0),
                    InterestRate = 2,
                }
            };

            return result;
        }
    }
}
