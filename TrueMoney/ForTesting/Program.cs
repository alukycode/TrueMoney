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
                var temp = context.Users.FirstOrDefault();
            }
        }
    }
}
