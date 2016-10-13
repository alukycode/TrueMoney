﻿using System;
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

        Task Add(User entity);

        Task<User> GetUserById(int id);

        Task<User> GetUserByAspNetId(string id);

        Task<User> GetUserByName(string name);

        Task<User> Create(
            int id,
            string email,
            string firstName,
            string lastName,
            string middleName,
            string passportSerie,
            string passportNumber,
            string passportGiveOrganisation,
            DateTime passportDateOfIssuing,
            string bankAccountNumber,
            string aspUserId);
    }
}