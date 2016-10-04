using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Infrastructure.Repositories
{
    public interface IUserProfileRepository
    {
        IEnumerable<UserProfile> GetAll();

        string GetShit();
    }
}
