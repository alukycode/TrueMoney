using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Models.Basic;
using TrueMoney.Models.Offer;

namespace TrueMoney.Services.Interfaces
{
    public interface IOfferService
    {
        Task<IList<OfferModel>> GetByUser(int userId);

        Task ApproveOffer(int offerId);

        Task CancelOfferApproval(int offerId);

        Task RevertOffer(int offerId);

        Task<CreateOfferForm> GetCreateOfferForm(int dealId, int currentUserId);

        Task CreateOffer(CreateOfferForm createOfferForm);
    }
}