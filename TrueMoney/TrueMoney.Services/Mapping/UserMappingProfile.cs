using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Account;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterViewModel, User>();
        }
    }
}
