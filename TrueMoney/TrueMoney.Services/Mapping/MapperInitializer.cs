using AutoMapper;

namespace TrueMoney.Services.Mapping
{
    public static class MapperInitializer
    {
        public static void Initialize(Profile profile)
        {
            //CreateMap<User, Entities.User>();
            Mapper.Initialize(
                conf =>
                {
                    conf.AddProfile(profile);
                });
        }

        public static void Initialize()
        {
            //CreateMap<User, Entities.User>();
            Mapper.Initialize(
                conf =>
                {
                    //conf.AddProfile<DataMappingProfile>();
                });
        }
    }
}
