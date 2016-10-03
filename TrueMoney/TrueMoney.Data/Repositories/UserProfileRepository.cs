using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;
using TrueMoney.Infrastructure.Repositories;

namespace TrueMoney.Data.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        public IEnumerable<UserProfile> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
