namespace ForTesting.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;
    using TrueMoney.Infrastructure.Repositories;

    public class UserRepository : IUserRepository
    {
        private List<User> Users = new List<User>();

        public UserRepository()
        {
            for (int i = 0; i < 10; i++)
            {
                this.Users.Add(new User
                {
                    Id = i,
                    Name = $"User {i}"
                });
            }
        }

        public Task<User> GetById(int id)
        {
            return new Task<User>(() => this.Users.FirstOrDefault(x => x.Id == id));
        }

        public Task<IList<User>> GetAll()
        {
            return new Task<IList<User>>(() => this.Users);
        }
    }
}