using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Account;
using TrueMoney.Models.Basic;
using TrueMoney.Models.User;

namespace TrueMoney.Services.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterViewModel, User>();
            CreateMap<UserModel, User>();
            CreateMap<User, UserModel>();
            CreateMap<EditUserViewModel, User>();
            CreateMap<User, EditUserViewModel>();
        }
    }
}
