using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Data.Mapping
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<User, Entities.User>();
            CreateMap<Entities.User, User>();
            CreateMap<Passport, Entities.Passport>();
            CreateMap<Entities.Passport, Passport>();
        }
    }
}
