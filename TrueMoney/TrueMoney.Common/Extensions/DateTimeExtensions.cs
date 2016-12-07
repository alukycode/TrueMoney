using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateWithTime(this DateTime dateTime)
        {
            return dateTime.ToString(StringFormats.DateWithTime);
        }
    }
}
