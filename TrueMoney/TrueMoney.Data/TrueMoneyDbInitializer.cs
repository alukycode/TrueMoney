using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TrueMoney.Common;
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
            // seed users and roles
            // http://stackoverflow.com/questions/19280527/mvc5-seed-users-and-roles
            var roleStore = new CustomRoleStore(context);
            var roleManager = new RoleManager<CustomRole, int>(roleStore);

            roleManager.Create(new CustomRole { Name = RoleNames.Admin });
            roleManager.Create(new CustomRole { Name = RoleNames.User });

            var userStore = new CustomUserStore(context);
            var userManager = new UserManager<User, int>(userStore);

            var users = GenerateUsers();
            foreach (var user in users)
            {
                userManager.Create(user);
                userManager.AddToRole(user.Id, RoleNames.User);
            }

            var admin = new User
            {
                Email = "admin@money.dev",
                UserName = "admin@money.dev",
                PasswordHash = new PasswordHasher().HashPassword("123123"),
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = "Admin",
                LastName = "Администратор",
                BankAccountNumber = "-",
                Passport = new Passport
                {
                    DateOfIssuing = DateTime.Now,
                    Number = "=",
                    Series = "-",
                }
            };

            userManager.Create(admin);
            userManager.AddToRole(admin.Id, RoleNames.Admin);

            context.SaveChanges();

            // seed other stuff

            var firsUser = users.First();
            firsUser.Deals = GenerateDeals();
            context.SaveChanges();
            GenerateOffers(context.Deals.ToList(), users.Skip(1).ToList());
            context.SaveChanges();
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
                    },
                    IsActive = true,
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
                    },
                    IsActive = true,
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

        private static void GenerateOffers(List<Deal> deals, List<User> offerers)
        {
            var offer = new Offer
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

            deals[1].Offers = new List<Offer>
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
                    InterestRate = 20,
                    IsApproved = true,
                },
                new Offer
                {
                    Offerer = offerers[0],
                    CreateTime = new DateTime(2016,10,09),
                    InterestRate = 21
                }
            };
        }

        private static PaymentPlan GeneratePlan()
        {
            return new PaymentPlan
            {
                CreateTime = DateTime.Now,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 10,
                        DueDate = DateTime.Now,
                    },
                    new Payment
                    {
                        Amount = 20,
                        DueDate = DateTime.Now,
                    }
                }
            };
        }

        private static List<Deal> GenerateDeals()
        {
            var result = new List<Deal>
            {
                new Deal
                {
                    Amount = 13,
                    CreateDate = DateTime.Now,
                    DealPeriod = 5000,
                    InterestRate = 12,
                    PaymentPlan = GeneratePlan(),
                    PaymentCount = 3,
                },
                new Deal
                {
                    Amount = 123,
                    CreateDate = DateTime.Now,
                    DealPeriod = 60,
                    PaymentCount = 3,
                    InterestRate = 2,
                },
                new Deal
                {
                    CreateDate = new DateTime(2016, 10, 09),
                    InterestRate = 25,
                    DealPeriod = 60,
                    PaymentCount = 3,
                    Description = "for business",
                    Amount = 100
                },
                new Deal
                {
                    Amount = 200,
                    CreateDate = new DateTime(2016, 10, 09),
                    InterestRate = 25,
                    DealPeriod = 60,
                    PaymentCount = 10,
                    Description = "to buy keyboard",
                },
                new Deal
                {
                    CreateDate = new DateTime(2016, 10, 09),
                    InterestRate = 5,
                    DealPeriod = 60,
                    PaymentCount = 5,
                    Description = "to rent a bitches",
                    Amount = 300
                }
            };

            return result;
        }
    }
}
