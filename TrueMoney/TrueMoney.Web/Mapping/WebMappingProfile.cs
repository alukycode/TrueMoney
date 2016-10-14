using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TrueMoney.Infrastructure.Entities;
using TrueMoney.Web.Models.Account;

namespace TrueMoney.Web.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.Passport, opt => opt.Ignore()); //первый рабочий пример
        }
    }
}