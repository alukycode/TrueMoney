using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Interfaces
{
    public interface IOfferService
    {
        Task<IList<OfferModel>> GetByUser(int userId);
    }
}