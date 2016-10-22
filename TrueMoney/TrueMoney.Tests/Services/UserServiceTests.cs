using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Services.Services;

namespace TrueMoney.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void Details_ByDefault_ReturnsUser()
        {
            // Create fake data
            var inMemoryItems = new FakeDbSet<User>
            {
                new User {Id = 1, FirstName = "One"},
                new User {Id = 2, FirstName = "Two"},
                new User {Id = 3, FirstName = "Three"},
            };

            // Create mock unit of work
            var mockData = new Mock<ITrueMoneyContext>();
            mockData.Setup(context => context.Users).Returns(inMemoryItems);

            // Setup service
            var userService = new UserService(mockData.Object);

            // Invoke
            var result = userService.GetDetails(1, 1).Result;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCurrentUser);
            Assert.AreEqual(inMemoryItems.First().FirstName, result.User.FirstName);
        }
    }
}
