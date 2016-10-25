namespace TrueMoney.Services.Mapping.TypeConverters
{
    using AutoMapper;

    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;

    public class OfferToOfferModelConverter : ITypeConverter<Offer, OfferModel>
    {
        public OfferModel Convert(Offer source, OfferModel destination, ResolutionContext context)
        {
            var currentUserId = (int)context.Items["currentUserId"];
            return new OfferModel
                       {
                           DealId = source.DealId,
                           Id = source.Id,
                           Rate = source.InterestRate,
                           IsCurrentUserLender = source.OffererId == currentUserId,
                           IsApproved = source.IsApproved,
                           Lender =
                               Mapper.Map<UserModel>(
                                   source.Offerer,
                                   opt => opt.Items["currentUserId"] = currentUserId),
                       };
        }
    }
}