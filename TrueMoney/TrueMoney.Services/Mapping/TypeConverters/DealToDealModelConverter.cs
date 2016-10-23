namespace TrueMoney.Services.Mapping.TypeConverters
{
    using AutoMapper;

    using TrueMoney.Common.Enums;
    using TrueMoney.Data.Entities;
    using TrueMoney.Models.Basic;

    public class DealToDealModelConverter : ITypeConverter<Deal, DealModel>
    {
        public DealModel Convert(Deal source, DealModel destination, ResolutionContext context)
        {
            var currentUserId = context.Items["currentUserId"];
            return new DealModel
            {
                //OwnerFullName = string.Concat(source.Owner.FirstName, " ", source.Owner.LastName),
                //Owner = Mapper.Map<UserModel>(source.Owner, opt => opt.Items["currentUserId"] = currentUserId),
                //Amount = source.Amount,
                //CreateDate = source.CreateDate,
                //DealPeriod = source.DealPeriod,
                //Description = source.Description,
                //IsInProgress = source.DealStatus == DealStatus.InProgress,
                //IsOpen = source.DealStatus == DealStatus.Open,
                //IsWaitForLoan = source.DealStatus == DealStatus.WaitForLoan,
                //IsWaitForApprove = source.DealStatus == DealStatus.WaitForApprove,
                //InterestRate = source.InterestRate,
                //Id = source.Id,
                //CloseDate = source.CloseDate
            };
        }
    }
}