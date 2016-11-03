using TrueMoney.Models;

namespace TrueMoney.Services.Mapping
{
    using System;
    using AutoMapper;

    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;
    using TrueMoney.Models.Deal;
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
            CreateMap<CreateDealForm, Deal>()
                .ForMember(
                    destination => destination.CreateDate,
                    member => member
                        .ResolveUsing(
                            x => DateTime.Now
                        ));
        }
    }
}