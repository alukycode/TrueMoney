using System.Linq;
using Moq;
using NUnit.Framework;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Services.Mapping;
using TrueMoney.Services.Services;

namespace TrueMoney.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        public UserServiceTests()
        {
            MapperInitializer.Initialize();
        }

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
            var userId = 1;

            // Create mock unit of work
            var mockData = new Mock<ITrueMoneyContext>();
            mockData.Setup(context => context.Users).Returns(inMemoryItems);

            // Setup service
            var userService = new UserService(mockData.Object);

            // Invoke
            var result = userService.GetDetails(userId).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(inMemoryItems.First().FirstName, result.User.FirstName);
        }
    }
}
