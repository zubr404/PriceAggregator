using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.VolatilityWeekly
{
    public class VolatilityDayModel
    {
        public decimal High { get; }
        public decimal Low { get; }
        public decimal? Percentage { get; }

        public VolatilityDayModel(decimal high, decimal low)
        {
            High = high;
            Low = low;
            Percentage = getPercentage();
        }

        private decimal? getPercentage()
        {
            if (High > 0)
            {
                return (1 - Low / High) * 100;
            }
            return null;
        }
    }
}
