using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Data.Repositories;
using TrueMoney.Infrastructure.Entities;
using TrueMoney.Mapping;

namespace ForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            MapperInitializer.Initialize();

            var rep = new UserRepository();
            var user = rep.GetById(1).Result;
            user.AspUserId = "not test";
            user.Passport.Number = "test";
            rep.Update(user).Wait();
        }
    }
}
