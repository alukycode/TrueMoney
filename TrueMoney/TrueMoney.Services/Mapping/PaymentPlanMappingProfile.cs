using AutoMapper;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Mapping
{
    public class PaymentPlanMappingProfile : Profile
    {
        public PaymentPlanMappingProfile()
        {
            CreateMap<PaymentPlan, PaymentPlanModel>();
            CreateMap<Payment, PaymentModel>();
        }
    }
}
