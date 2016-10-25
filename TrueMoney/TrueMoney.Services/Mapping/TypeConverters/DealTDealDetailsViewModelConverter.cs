namespace TrueMoney.Services.Mapping.TypeConverters
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;
    using TrueMoney.Models.ViewModels;

    public class DealTDealDetailsViewModelConverter : ITypeConverter<Deal, DealDetailsViewModel>
    {
        public DealDetailsViewModel Convert(Deal source, DealDetailsViewModel destination, ResolutionContext context)
        {
            var currentUserId = (int)context.Items["currentUserId"];
            var currentUserOffer = source.Offers.FirstOrDefault(x => x.OffererId == currentUserId);

            return new DealDetailsViewModel
                       {
                           IsCurrentUserBorrower = source.OwnerId == currentUserId,
                           IsCurrentUserLender = source.Offers.Any(x => x.OffererId == currentUserId),
                           Deal = Mapper.Map<DealModel>(source, opt => opt.Items["currentUserId"] = currentUserId),
                           CurrentUserId = currentUserId,
                           Offers = Mapper.Map<IList<OfferModel>>(source.Offers, opt => opt.Items["currentUserId"] = currentUserId),
                           CurrentUserOffer = currentUserOffer != null
                                   ? Mapper.Map<OfferModel>(
                                       currentUserOffer,
                                       opt => opt.Items["currentUserId"] = currentUserId)
                                   : null
                       };
        }
    }
}