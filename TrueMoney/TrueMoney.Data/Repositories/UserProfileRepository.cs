using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;
using TrueMoney.Infrastructure.Repositories;

namespace TrueMoney.Data.Repositories
{
    public class UserProfileRepository : IUserRepository
    {
        public User GetById(int Id)
        {
            throw new NotImplementedException();
        }

        IList<User> IRepository<User>.GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public string GetShit()
        {
            return "Repository";
        }
    }
}
