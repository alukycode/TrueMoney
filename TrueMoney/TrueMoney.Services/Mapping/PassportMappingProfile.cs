using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Account;

namespace TrueMoney.Services.Mapping
{
    public class PassportMappingProfile : Profile
    {
        public PassportMappingProfile()
        {
            CreateMap<PassportViewModel, Passport>();
        }
    }
}
