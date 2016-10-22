using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Services.Mapping;

namespace ForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TrueMoneyContext())
            {
                var temp = context.Users.FirstAsync(x => x.AspUserId == "38fa30b4-9ffc-4e8d-a346-f907dd0d5169").Result;
                var s = temp.Id;
            }
        }
    }
}
