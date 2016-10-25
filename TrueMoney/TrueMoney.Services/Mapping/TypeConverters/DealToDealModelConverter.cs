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
                BorrowerFullName = string.Concat(source.Owner.FirstName, " ", source.Owner.LastName),
                Borrower = Mapper.Map<UserModel>(source.Owner, opt => opt.Items["currentUserId"] = currentUserId),
                Amount = source.Amount,
                CreateDate = source.CreateDate,
                DayCount = source.DealPeriod.Days,
                Description = source.Description,
                IsInProgress = source.DealStatus == DealStatus.InProgress,
                IsOpen = source.DealStatus == DealStatus.Open,
                IsWaitForLoan = source.DealStatus == DealStatus.WaitForLoan,
                IsWaitForApprove = source.DealStatus == DealStatus.WaitForApprove,
                Rate = source.InterestRate,
                Id = source.Id,
                CloseDate = source.CloseDate
            };
        }
    }
}