using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    public interface IUserService
    {
        Task Add(UserModel entity);
        Task<UserModel> Create(int id, string email, string firstName, string lastName, string middleName, string passportSerie, string passportNumber, string passportGiveOrganisation, DateTime passportDateOfIssuing, string bankAccountNumber, string aspUserId);
        Task<IEnumerable<UserModel>> GetAll();
        Task<UserModel> GetByAspId(string id);
        Task<UserModel> GetById(int id);
        Task<UserModel> GetUserByName(string name);
    }
}