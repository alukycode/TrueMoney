using System;
using System.ComponentModel;
using System.Linq;

namespace TrueMoney.Common.Enums
{
    public enum DealStatus
    {
        [Description("Открыт для предложений")]
        Open = 0,

        [Description("Ожидается подтверждение от кредитора")]
        WaitForApprove = 1,

        [Description("Ожидается перевод денег от кредитора")]
        WaitForLoan = 2,

        [Description("Заёмщик постепенно возвращает сумму займа")]
        InProgress = 3,

        [Description("Займ завершён")]
        Closed = 4
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
