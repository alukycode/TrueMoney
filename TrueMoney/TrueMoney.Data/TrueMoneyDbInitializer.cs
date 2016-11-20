﻿using System;
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

            users[0].Deals = GenerateDealsWithOneInProgress(users.Where(x => x != users[0]).ToList());
            users[1].Deals = GenerateDealsWithOneOpen(users.Where(x => x != users[1]).ToList());
            users[2].Deals = GenerateDealsWithOneWaitForApprove(users.Where(x => x != users[2]).ToList());
            users[3].Deals = GenerateDealsWithOneWaitForLoan(users.Where(x => x != users[3]).ToList());
            users[4].Deals = GenerateDealsWithOneOpen(users.Where(x => x != users[4]).ToList());

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
                    BankAccountNumber = "408.17.810.0.9991.000000",
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
                    BankAccountNumber = "408.17.810.0.9991.000001",
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
                    BankAccountNumber = "408.17.810.0.9991.000002",
                    IsActive = true,
                },
                new User
                {
                    Email    = "test@example.com",
                    UserName = "test@example.com",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Test",
                    LastName = "Example",
                    BankAccountNumber = "408.17.810.0.9991.000003",
                    IsActive = true,
                },
                new User
                {
                    Email    = "qwe@asd.zxc",
                    UserName = "qwe@asd.zxc",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Qwe",
                    LastName = "Asd",
                    BankAccountNumber = "408.17.810.0.9991.000004",
                },
            };
        }

        #region Offers generation
        private static List<Offer> GenerateOffersWithOneApproved(
            List<User> offerers,
            DateTime dealCreateDate,
            int finalRate)
        {
            var result = new List<Offer>
            {
                new Offer
                {
                    CreateTime = dealCreateDate.AddDays(2),
                    InterestRate = finalRate,
                    IsApproved = true,
                    Offerer = offerers[0],
                },
                new Offer
                {
                    CreateTime = dealCreateDate.AddDays(1),
                    InterestRate = finalRate + 1,
                    Offerer = offerers[1],
                },
            };

            return result;
        }

        private static List<Offer> GenerateOffers(
            List<User> offerers,
            DateTime dealCreateDate,
            int finalRate)
        {
            var result = new List<Offer>
            {
                new Offer
                {
                    CreateTime = dealCreateDate.AddDays(2),
                    InterestRate = finalRate,
                    Offerer = offerers[0],
                },
                new Offer
                {
                    CreateTime = dealCreateDate.AddDays(3),
                    InterestRate = finalRate + 2,
                    Offerer = offerers[1],
                },
            };

            return result;
        } 
        #endregion

        #region Payment plans generation
        private static PaymentPlan GenerateOpenPlan(DateTime planCreateDate, decimal paymentsAmount)
        {
            return new PaymentPlan
            {
                CreateTime = planCreateDate,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = paymentsAmount / 2,
                        DueDate = planCreateDate.AddDays(10),
                    },
                    new Payment
                    {
                        Amount = paymentsAmount / 2,
                        DueDate = planCreateDate.AddDays(20),
                    }
                }
            };
        }

        private static PaymentPlan GenerateClosedPlan(DateTime planCreateDate, decimal paymentsAmount)
        {
            var payments = new List<Payment>
            {
                new Payment
                {
                    Amount = paymentsAmount / 2,
                    DueDate = planCreateDate.AddDays(10),
                    IsPaid = true,
                    PaidDate = planCreateDate.AddDays(10),
                },
                new Payment
                {
                    Amount = paymentsAmount / 2,
                    DueDate = planCreateDate.AddDays(20),
                    IsPaid = true,
                    PaidDate = planCreateDate.AddDays(10),
                }
            };

            return new PaymentPlan
            {
                CreateTime = planCreateDate,
                Payments = payments,
                BankTransactions = GenerateBankTransactions(payments),
            };
        } 
        #endregion

        #region Transactions generation
        private static List<BankTransaction> GenerateBankTransactions(List<Payment> payments)
        {
            var result = new List<BankTransaction>();
            foreach (var item in payments)
            {
                result.Add(new BankTransaction
                {
                    Amount = item.Amount,
                    DateOfPayment = item.DueDate,
                });
            }

            return result;
        } 
        #endregion

        #region Deals generation
        private static List<Deal> GenerateDealsWithOneInProgress(List<User> offerers)
        {
            var firstDealCreateDate = new DateTime(2012, 12, 12);
            var SecondDealCreateDate = new DateTime(2013, 12, 12);
            var firstRate = 12;
            var secondRate = 30;
            decimal firstAmount = 1500;
            decimal secondAmount = 13;
            var result = new List<Deal>
            {
                new Deal
                {
                    Amount = firstAmount,
                    CreateDate = firstDealCreateDate,
                    DealPeriod = 60,
                    PaymentCount = 2,
                    InterestRate = firstRate,
                    DealStatus = DealStatus.Closed,
                    PaymentPlan = GenerateClosedPlan(firstDealCreateDate.AddDays(3), firstAmount * (1 + (decimal)firstRate / 100)),
                    Offers = GenerateOffersWithOneApproved(offerers, firstDealCreateDate, firstRate),
                    CloseDate = firstDealCreateDate.AddDays(60),
                },
                new Deal
                {
                    Amount = secondAmount,
                    CreateDate = SecondDealCreateDate,
                    DealPeriod = 30,
                    InterestRate = secondRate,
                    PaymentPlan = GenerateOpenPlan(SecondDealCreateDate.AddDays(3), secondAmount * (1 + (decimal)secondRate / 100)),
                    PaymentCount = 2,
                    DealStatus = DealStatus.InProgress,
                    Offers = GenerateOffersWithOneApproved(offerers, SecondDealCreateDate, secondRate),
                },
            };

            return result;
        }

        private static List<Deal> GenerateDealsWithOneOpen(List<User> offerers)
        {
            var firstDealCreateDate = new DateTime(2012, 12, 12);
            var SecondDealCreateDate = new DateTime(2013, 12, 12);
            var firstRate = 12;
            var SecondRate = 30;
            decimal firstAmount = 1000;
            var result = new List<Deal>
            {
                new Deal
                {
                    Amount = firstAmount,
                    CreateDate = firstDealCreateDate,
                    DealPeriod = 60,
                    PaymentCount = 2,
                    InterestRate = firstRate,
                    DealStatus = DealStatus.Closed,
                    PaymentPlan = GenerateClosedPlan(firstDealCreateDate.AddDays(3), firstAmount * (1 + (decimal)firstRate / 100)),
                    Offers = GenerateOffersWithOneApproved(offerers, firstDealCreateDate, firstRate),
                    CloseDate = firstDealCreateDate.AddDays(60),
                },
                new Deal
                {
                    Amount = 2000,
                    CreateDate = SecondDealCreateDate,
                    DealPeriod = 30,
                    InterestRate = SecondRate,
                    PaymentCount = 2,
                    DealStatus = DealStatus.Open,
                    Offers = GenerateOffers(offerers, SecondDealCreateDate, SecondRate),
                },
            };

            return result;
        }

        private static List<Deal> GenerateDealsWithOneWaitForApprove(List<User> offerers)
        {
            var firstDealCreateDate = new DateTime(2012, 12, 12);
            var SecondDealCreateDate = new DateTime(2013, 12, 12);
            var firstRate = 12;
            var SecondRate = 30;
            decimal firstAmount = 123;
            var result = new List<Deal>
            {
                new Deal
                {
                    Amount = firstAmount,
                    CreateDate = firstDealCreateDate,
                    DealPeriod = 60,
                    PaymentCount = 2,
                    InterestRate = firstRate,
                    DealStatus = DealStatus.Closed,
                    PaymentPlan = GenerateClosedPlan(firstDealCreateDate.AddDays(3), firstAmount * (1 + (decimal)firstRate / 100)),
                    Offers = GenerateOffersWithOneApproved(offerers, firstDealCreateDate, firstRate),
                    CloseDate = firstDealCreateDate.AddDays(60),
                },
                new Deal
                {
                    Amount = 13,
                    CreateDate = SecondDealCreateDate,
                    DealPeriod = 30,
                    InterestRate = SecondRate,
                    PaymentCount = 2,
                    DealStatus = DealStatus.WaitForApprove,
                    Offers = GenerateOffersWithOneApproved(offerers, SecondDealCreateDate, SecondRate),
                },
            };

            return result;
        }

        private static List<Deal> GenerateDealsWithOneWaitForLoan(List<User> offerers)
        {
            var firstDealCreateDate = new DateTime(2012, 12, 12);
            var SecondDealCreateDate = new DateTime(2013, 12, 12);
            var firstRate = 12;
            var SecondRate = 30;
            decimal firstAmount = 123;
            var result = new List<Deal>
            {
                new Deal
                {
                    Amount = firstAmount,
                    CreateDate = firstDealCreateDate,
                    DealPeriod = 60,
                    PaymentCount = 2,
                    InterestRate = firstRate,
                    DealStatus = DealStatus.Closed,
                    PaymentPlan = GenerateClosedPlan(firstDealCreateDate.AddDays(3), firstAmount * (1 + (decimal)firstRate / 100)),
                    Offers = GenerateOffersWithOneApproved(offerers, firstDealCreateDate, firstRate),
                    CloseDate = firstDealCreateDate.AddDays(60),
                },
                new Deal
                {
                    Amount = 13,
                    CreateDate = SecondDealCreateDate,
                    DealPeriod = 30,
                    InterestRate = SecondRate,
                    PaymentCount = 2,
                    DealStatus = DealStatus.WaitForLoan,
                    Offers = GenerateOffersWithOneApproved(offerers, SecondDealCreateDate, SecondRate),
                },
            };

            return result;
        } 
        #endregion
    }
}
