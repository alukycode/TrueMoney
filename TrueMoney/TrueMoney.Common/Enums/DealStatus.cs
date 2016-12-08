using System.ComponentModel;

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
}
