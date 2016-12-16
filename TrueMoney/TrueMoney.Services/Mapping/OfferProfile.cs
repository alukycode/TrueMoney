namespace TrueMoney.Services.Mapping
{
    using System;
    using AutoMapper;
    using Common.Enums;
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
                    member => member.ResolveUsing(x => x.Offerer.Rating))
                .ForMember(
                    destination => destination.DealStatus,
                    member => member.ResolveUsing(x => x.Deal != null? x.Deal.DealStatus : default(DealStatus)))
                .ForMember(
                    destination => destination.DealAmount,
                    member => member.ResolveUsing(x => x.Deal != null ? x.Deal.Amount : 0));

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