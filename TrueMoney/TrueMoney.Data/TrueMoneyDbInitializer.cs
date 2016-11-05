using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
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
            List<User> users = GenerateUsers();
            foreach (var item in users)
            {
                context.Users.Add(item);
            }
            context.SaveChanges();

            var user = users.First();
            user.Deals = GenerateDeals();
            context.SaveChanges();

            GenerateOffers(context.Deals.ToList(), users.Skip(1).ToList());
            context.SaveChanges();
        }

        private static void GenerateOffers(List<Deal> deals, List<User> offerers)
        {
            var offer = new Offer()
            {
                CreateTime = DateTime.Now,
                InterestRate = 10,
                IsApproved = true,
                Offerer = offerers[0],
            };
            var deal = deals.First();
            deal.Offers = new List<Offer>
            {
                offer,
                new Offer
                {
                    CreateTime = DateTime.Now,
                    InterestRate = 20,
                    IsApproved = true,
                    Offerer = offerers[0],
                }
            };
            deal.ResultOffer = offer;
            deal.DealStatus = DealStatus.InProgress;

            deals[1].Offers = new List<Offer>()
            {
                new Offer
                {
                    CreateTime = DateTime.Now,
                    InterestRate = 20,
                    IsApproved = true,
                    Offerer = offerers[0],
                }
            };

            deals[2].Offers = new List<Offer>
            {
                new Offer
                {
                    Offerer = offerers[1],
                    CreateTime = new DateTime(2016,10,09),
                    InterestRate = 20
                },
                new Offer
                {
                    Offerer = offerers[0],
                    CreateTime = new DateTime(2016,10,09),
                    InterestRate = 21
                }
            };
        }

        private static List<User> GenerateUsers()
        {
            var defaultPasswordHash = new PasswordHasher().HashPassword("123123");

            return new List<User>
            {
                new User
                {
                    Email    = "facepalm@money.dev",
                    UserName = "facepalm@money.dev",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Саша",
                    LastName = "Черногребель",
                    BankAccountNumber = "test",
                    Passport = new Passport
                    {
                        DateOfIssuing = DateTime.Now,
                        Number = "test",
                        Series = "test",
                    }
                },
                new User
                {
                    Email    = "anton@money.dev",
                    UserName = "anton@money.dev",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Антон",
                    LastName = "Лукьянов",
                    BankAccountNumber = "test",
                    Passport = new Passport
                    {
                        DateOfIssuing = DateTime.Now,
                        Number = "test",
                        Series = "test",
                    }
                },
                new User
                {
                    Email    = "dimon@money.dev",
                    UserName = "dimon@money.dev",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Дима",
                    LastName = "Артюх",
                    BankAccountNumber = "test",
                },
                new User
                {
                    Email    = "test@example.com",
                    UserName = "test@example.com",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Test",
                    LastName = "Example",
                    BankAccountNumber = "100500",
                },
                new User
                {
                    Email    = "qwe@asd.zxc",
                    UserName = "qwe@asd.zxc",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Qwe",
                    LastName = "Asd",
                    BankAccountNumber = "100500",
                },
            };
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

        private static List<Deal> GenerateDeals()
        {
            var result = new List<Deal>()
            {
                new Deal()
                {
                    Amount = 13,
                    CreateDate = DateTime.Now,
                    DealPeriod = 5000,
                    InterestRate = 12,
                    PaymentPlan = GeneratePlan()
                },
                new Deal()
                {
                    Amount = 123,
                    CreateDate = DateTime.Now,
                    DealPeriod = 60,
                    InterestRate = 2,
                },
                new Deal
                {
                    CreateDate = new DateTime(2016, 10, 09),
                    InterestRate = 25,
                    Description = "for business",
                    Amount = 100
                },
                new Deal
                {
                    Amount = 200,
                    CreateDate = new DateTime(2016, 10, 09),
                    InterestRate = 25,
                    Description = "to buy keyboard",                    
                },
                new Deal
                {
                    CreateDate = new DateTime(2016, 10, 09),
                    InterestRate = 5,
                    Description = "to rent a bitches",
                    Amount = 300
                }
            };

            return result;
        }
    }
}
