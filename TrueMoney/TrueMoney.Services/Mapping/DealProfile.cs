namespace TrueMoney.Services.Mapping
{
    using AutoMapper;

    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;
    using TrueMoney.Models.ViewModels;

    public class DealProfile : Profile
    {
        public DealProfile()
        {
            CreateMap<Deal, DealModel>()
                .ForMember(
                    destination => destination.OwnerFullName,
                    member => member
                        .ResolveUsing(
                            x => $"{x.Owner.FirstName} {x.Owner.LastName}"
                        ));
            CreateMap<Deal, DealDetailsViewModel>();
        }
    }
}