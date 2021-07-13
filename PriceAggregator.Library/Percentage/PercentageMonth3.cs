using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.Extensions;
using PriceAggregator.Library.Interfaces;

namespace PriceAggregator.Library.Percentage
{
    public class PercentageMonth3 : IPercentage
    {
        private readonly IRepository repository;
        private readonly string simbol;
        private readonly string interval;

        public PercentageMonth3(IRepository repository, string simbol, string interval)
        {
            this.repository = repository;
            this.simbol = simbol;
            this.interval = interval;
        }

        public PercentageChange GetPercentage()
        {
            return this.GetPercentageChange(repository, simbol, KlineTimeframe.month1, interval, 3);
        }
    }
}
