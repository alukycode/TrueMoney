using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Infrastructure.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();

        Task<User> GetCurrentUser();
        Task<User> GetUserById(int id);
        Task<User> GetUserByName(string name);

        Task<User> Create(
            int id,
            string email,
            string firstName,
            string lastName,
            string familyName,
            string passportSeria,
            string passportNumber,
            string passportGiveOrganisation,
            DateTime passportGiveTime,
            string bankAccountNumber);
    }
}
