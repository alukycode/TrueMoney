using AutoMapper;

namespace TrueMoney.Services.Mapping
{
    public static class MapperInitializer
    {
        public static void Initialize()
        {
            Mapper.Initialize(
                conf =>
                {
                    conf.AddProfile<UserMappingProfile>();
                    conf.AddProfile<PassportMappingProfile>();
                    conf.AddProfile<DealProfile>();
                    conf.AddProfile<OfferProfile>();
                });
        }
    }
}
