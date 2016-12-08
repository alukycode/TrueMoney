using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Common.Extensions
{
    public static class DecimalExtensions
    {
        public static string Format(this decimal value)
        {
            return value.ToString("0.##");
        }
    }
}
