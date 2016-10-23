namespace TrueMoney.Services.Mapping
{
    using AutoMapper;

    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;
    using TrueMoney.Services.Mapping.TypeConverters;

    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<Offer, OfferModel>();
        }
    }
}