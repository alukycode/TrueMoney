using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data.Mapping;

namespace TrueMoney.Mapping
{
    public static class MapperInitializer
    {
        public static void Initialize(Profile profile)
        {
            Mapper.Initialize(
                conf =>
                {
                    conf.AddProfile<DataMappingProfile>();
                    conf.AddProfile(profile);
                });
        }

        public static void Initialize()
        {
            Mapper.Initialize(
                conf =>
                {
                    conf.AddProfile<DataMappingProfile>();
                });
        }
    }
}
