using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.Extensions
{
    public static class ConverterValues
    {
        public static string DecimalToString(this decimal number, int digits)
        {
            var numberRound = Math.Round(number, digits);
            return numberRound.ToString();
        }
    }
}
