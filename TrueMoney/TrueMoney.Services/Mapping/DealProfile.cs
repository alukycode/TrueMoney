namespace TrueMoney.Services.Mapping
{
    using AutoMapper;

    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;
    using TrueMoney.Models.ViewModels;
    using TrueMoney.Services.Mapping.TypeConverters;

    public class DealProfile : Profile
    {
        public DealProfile()
        {
            CreateMap<Deal, DealModel>().ConvertUsing<DealToDealModelConverter>();
            CreateMap<Deal, DealDetailsViewModel>().ConvertUsing<DealTDealDetailsViewModelConverter>();
        }
    }
}