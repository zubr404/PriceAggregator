using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.GreenRedPercentage
{
    public class GreenRedPercentChange
    {
        public string Simbol { get; set; }
        public string Interval { get; set; }
        public decimal? PercentageGreen { get; set; }
        public decimal? PercentageRed { get; set; }
    }
}
