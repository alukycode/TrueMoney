using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Infrastructure.Entities
{
    public enum DealStatus
    {
        Open,
        WaitForLoan,
        InProgress,
        Closed
    }
}
