namespace TrueMoney.Services.Mapping
{
    using System;
    using AutoMapper;
    using Models.Offer;
    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;

    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<Offer, OfferModel>()
                .ForMember(
                    destination => destination.OffererFullName,
                    member => member.ResolveUsing(x => $"{x.Offerer.FirstName} {x.Offerer.LastName}"))
                .ForMember(
                destination => destination.Rating,
                    member => member.ResolveUsing(x => x.Offerer.Rating));

            CreateMap<CreateOfferForm, Offer>()
                .ForMember(
                    destination => destination.CreateTime,
                    member => member
                        .ResolveUsing(
                            x => DateTime.Now
                        ));
        }
    }
}