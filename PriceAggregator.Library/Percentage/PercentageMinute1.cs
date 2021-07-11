using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Percentage
{
    public class PercentageMinute1 : IPercentage
    {
        private readonly CandlesRepository repository;
        private readonly string simbol;

        public PercentageMinute1(CandlesRepository repository, string simbol)
        {
            this.repository = repository;
            this.simbol = simbol;
        }

        public PercentageChange GetPercentage()
        {
            var candles = repository.Get(simbol, KlineTimeframe.minute1);
            var percentage = PercentageMatchingTimeframes.GetPercentage(candles);
            if (percentage.HasValue)
            {
                return new PercentageChange()
                {
                    Interval = KlineTimeframe.minute1,
                    Percentage = percentage.Value,
                    Simbol = simbol
                };
            }
            return null;
        }
    }
}
