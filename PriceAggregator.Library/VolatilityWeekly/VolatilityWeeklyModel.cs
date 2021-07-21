using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.VolatilityWeekly
{
    public class VolatilityWeeklyModel
    {
        public string Simbol { get; }
        public List<VolatilityDayModel> VolatilityDayModels { get; }
        public decimal High { get; private set; }
        public decimal Low { get; private set; }
        public decimal? Percentage { get; private set; }

        public VolatilityWeeklyModel(string simbol)
        {
            Simbol = simbol;
            VolatilityDayModels = new List<VolatilityDayModel>();
        }

        public void SetResults()
        {
            if (VolatilityDayModels.Count > 0)
            {
                High = VolatilityDayModels.Max(x => x.High);
                Low = VolatilityDayModels.Min(x => x.Low);
                Percentage = getPercentage();
            }
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
