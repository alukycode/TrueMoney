using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Data.Repositories;
using TrueMoney.Mapping;

namespace ForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TrueMoneyContext())
            {
                context.Users.Add(new User() { Name = "temp" });
                context.SaveChanges();
            }
        }
    }
}
