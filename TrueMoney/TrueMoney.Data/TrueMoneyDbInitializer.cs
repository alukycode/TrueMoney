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
        private static readonly Random _random = new Random(524287);

        private static readonly List<string> _lastNames = new List<string>
        {
            "Мисюкевич",
            "Жук",
            "Волчек",
            "Соловьёв",
            "Буликов",
            "Гончарик",
            "Сманцер",
            "Соболь",
            "Романов",
            "Ковальчук",
            "Вакульчик",
            "Хохлов",
            "Обуховский",
        };

        private static readonly List<string> _firstNames = new List<string>
        {
            "Андрей",
            "Антон",
            "Андрей",
            "Александр",
            "Владислав",
            "Алексей",
            "Дмитрий",
            "Андрей",
            "Антон",
            "Сергей",
            "Роман",
            "Артур",
            "Евгений",
        };

        private static readonly List<string> _aims = new List<string>
        {
            "На квартиру",
            "На машину",
            "Для благотворительности",
            "",
            "Для развития бизнеса",
            "На развитие стартапа",
            "Для личных целей",
            "На путешествия",
        };

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
                CardNumber = "-",
            };

            userManager.Create(admin);
            userManager.AddToRole(admin.Id, RoleNames.Admin);
            context.SaveChanges();

            // seed other stuff

            users[0].Deals = GenerateDealsWithOneInProgress(users.Where(x => x != users[0] && x.IsActive).ToList());
            users[1].Deals = GenerateDealsWithOneOpen(users.Where(x => x != users[1] && x.IsActive).ToList());
            users[2].Deals = GenerateDealsWithOneWaitForApprove(users.Where(x => x != users[2] && x.IsActive).ToList());
            users[3].Deals = GenerateDealsWithOneWaitForLoan(users.Where(x => x != users[3] && x.IsActive).ToList());

            foreach (var item in users.Skip(5))
            {
                item.Deals = GenerateDealsWithOneOpen(users.Where(x => x != item).ToList());
            }

            context.SaveChanges();
        }

        #region Users generation
        private static List<User> GenerateUsers()
        {
            var defaultPasswordHash = new PasswordHasher().HashPassword("123123");
            var users = new List<User>
            {
                new User
                {
                    Email    = "facepalm@money.dev",
                    UserName = "facepalm@money.dev",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Саша",
                    LastName = "Черногребель",
                    CardNumber = GenerateCardNumber(),
                    Passport = GeneratePassport(0),
                    IsActive = true,
                    LockoutEnabled = true,
                    Rating = 0,
                },
                new User
                {
                    Email    = "anton@money.dev",
                    UserName = "anton@money.dev",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Антон",
                    LastName = "Лукьянов",
                    CardNumber = GenerateCardNumber(),
                    Passport = GeneratePassport(1),
                    IsActive = true,
                    LockoutEnabled = true,
                    Rating = 0,
                },
                new User
                {
                    Email    = "dimon@money.dev",
                    UserName = "dimon@money.dev",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Дима",
                    LastName = "Артюх",
                    CardNumber = GenerateCardNumber(),
                    Passport = GeneratePassport(2),
                    IsActive = true,
                    LockoutEnabled = true,
                    Rating = 0,
                },
                new User
                {
                    Email    = "test@example.com",
                    UserName = "test@example.com",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Test",
                    LastName = "Example",
                    CardNumber = GenerateCardNumber(),
                    Passport = GeneratePassport(3),
                    IsActive = true,
                    LockoutEnabled = true,
                },
                new User
                {
                    Email    = "qwe@asd.zxc",
                    UserName = "qwe@asd.zxc",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Неактивный",
                    LastName = "Единственный",
                    CardNumber = GenerateCardNumber(),
                    Passport = GeneratePassport(4),
                    LockoutEnabled = true,
                },
            };

            users.AddRange(GenerateFakeUsers(defaultPasswordHash));

            return users;
        }

        private static string GenerateCardNumber()
        {
            return
                $"{_random.Next(0, 9999).ToString("D4")} {_random.Next(0, 9999).ToString("D4")} {_random.Next(0, 9999).ToString("D4")} {_random.Next(0, 9999).ToString("D4")}";
        }

        private static Passport GeneratePassport(int number)
        {
            return new Passport
            {
                DateOfIssuing = new DateTime(
                    1950 + _random.Next(60),
                    _random.Next(1, 12),
                    _random.Next(1, 20)),
                Number = "KB" + (number).ToString("D6"),
                GiveOrganization = number <= 7 ? "Первомайский ГОВД г. Бобруйска" : "Ленинский РУВД г.Минска",
                ImagePath = number >= 0 && number <= 7 ? "/Images/Passport/" + number + ".jpg" : null
            };
        }

        private static IEnumerable<User> GenerateFakeUsers(string defaultPasswordHash)
        {
            List<User> users = new List<User>();
            
            for (int i = 0; i < _firstNames.Count; i++)
            {
                users.Add(new User
                {
                    Email = $"fake{i}@money.dev",
                    UserName = $"fake{i}@money.dev",
                    PasswordHash = defaultPasswordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = _firstNames[i],
                    LastName = _lastNames[i],
                    CardNumber = GenerateCardNumber(),
                    Passport = GeneratePassport(i + 5),
                    IsActive = true,
                    LockoutEnabled = true,
                    Rating = _random.Next(-3, 3),
                });
            }

            return users;
        } 
        #endregion

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
                    Offerer = offerers[_random.Next(offerers.Count - 1)],
                },
                new Offer
                {
                    CreateTime = dealCreateDate.AddDays(1),
                    InterestRate = finalRate,
                    Offerer = offerers[_random.Next(offerers.Count - 1)],
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
                    Offerer = offerers[_random.Next(offerers.Count - 1)],
                },
                new Offer
                {
                    CreateTime = dealCreateDate.AddDays(3),
                    InterestRate = finalRate,
                    Offerer = offerers[_random.Next(offerers.Count - 1)],
                },
            };

            return result;
        }
        #endregion

        #region Payment plans generation
        private static PaymentPlan GenerateOpenPlan(
            DateTime planCreateDate,
            decimal paymentsAmount,
            int dealPeriod)
        {
            return new PaymentPlan
            {
                CreateTime = planCreateDate,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = paymentsAmount / 2,
                        DueDate = planCreateDate.AddDays(dealPeriod / 2),
                    },
                    new Payment
                    {
                        Amount = paymentsAmount / 2,
                        DueDate = planCreateDate.AddDays(dealPeriod),
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
            var firstDealCreateDate = new DateTime(2010 + _random.Next(5), _random.Next(1, 12), _random.Next(1, 20));
            var secondDealCreateDate = DateTime.Now.AddDays(-10);
            var firstRate = _random.Next(1, 50);
            var secondRate = _random.Next(1, 50);
            decimal firstAmount = _random.Next(100, 5000);
            decimal secondAmount = _random.Next(100, 5000);
            var secondDealPeriod = 30;

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
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
                new Deal
                {
                    Amount = secondAmount,
                    CreateDate = secondDealCreateDate,
                    DealPeriod = secondDealPeriod,
                    InterestRate = secondRate,
                    PaymentPlan = GenerateOpenPlan(
                        secondDealCreateDate.AddDays(3),
                        secondAmount * (1 + (decimal)secondRate / 100),
                        secondDealPeriod),
                    PaymentCount = 2,
                    DealStatus = DealStatus.InProgress,
                    Offers = GenerateOffersWithOneApproved(offerers, secondDealCreateDate, secondRate),
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
            };

            return result;
        }

        private static List<Deal> GenerateDealsWithOneOpen(List<User> offerers)
        {
            var firstDealCreateDate = new DateTime(2010 + _random.Next(5), _random.Next(1, 12), _random.Next(1, 20));
            var secondDealCreateDate = firstDealCreateDate.AddYears(1);
            var firstRate = _random.Next(1, 50);
            var secondRate = _random.Next(1, 50);
            decimal firstAmount = _random.Next(100, 5000);
            decimal secondAmount = _random.Next(100, 5000);
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
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
                new Deal
                {
                    Amount = secondAmount,
                    CreateDate = secondDealCreateDate,
                    DealPeriod = 30,
                    InterestRate = secondRate,
                    PaymentCount = 2,
                    DealStatus = DealStatus.Open,
                    Offers = GenerateOffers(offerers, secondDealCreateDate, secondRate),
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
            };

            return result;
        }

        private static List<Deal> GenerateDealsWithOneWaitForApprove(List<User> offerers)
        {
            var firstDealCreateDate = new DateTime(2010 + _random.Next(5), _random.Next(1, 12), _random.Next(1, 20));
            var secondDealCreateDate = firstDealCreateDate.AddYears(1);
            var firstRate = _random.Next(1, 50);
            var secondRate = _random.Next(1, 50);
            decimal firstAmount = _random.Next(100, 5000);
            decimal secondAmount = _random.Next(100, 5000);
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
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
                new Deal
                {
                    Amount = secondAmount,
                    CreateDate = secondDealCreateDate,
                    DealPeriod = 30,
                    InterestRate = secondRate,
                    PaymentCount = 2,
                    DealStatus = DealStatus.WaitForApprove,
                    Offers = GenerateOffersWithOneApproved(offerers, secondDealCreateDate, secondRate),
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
            };

            return result;
        }

        private static List<Deal> GenerateDealsWithOneWaitForLoan(List<User> offerers)
        {
            var firstDealCreateDate = new DateTime(2010 + _random.Next(5), _random.Next(1, 12), _random.Next(1, 20));
            var secondDealCreateDate = firstDealCreateDate.AddYears(1);
            var firstRate = _random.Next(1, 50);
            var secondRate = _random.Next(1, 50);
            decimal firstAmount = _random.Next(100, 5000);
            decimal secondAmount = _random.Next(100, 5000);
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
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
                new Deal
                {
                    Amount = secondAmount,
                    CreateDate = secondDealCreateDate,
                    DealPeriod = 30,
                    InterestRate = secondRate,
                    PaymentCount = 2,
                    DealStatus = DealStatus.WaitForLoan,
                    Offers = GenerateOffersWithOneApproved(offerers, secondDealCreateDate, secondRate),
                    Description = _aims[_random.Next(_aims.Count - 1)],
                },
            };

            return result;
        } 
        #endregion
    }
}
