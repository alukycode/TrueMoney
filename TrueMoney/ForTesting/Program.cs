using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bank.BankApi;
using Bank.Resources;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Services.Mapping;

namespace ForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            BankDataHelper.UpdateDataFile();
            var x = BankDataHelper.GetAccounts();
        }
    }
}
