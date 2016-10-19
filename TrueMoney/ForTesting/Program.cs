using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Data.Repositories;
using TrueMoney.Services.Mapping;

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
            user.Passport.Number = "not wtf? test";
            rep.Update(user).Wait();
        }
    }
}
