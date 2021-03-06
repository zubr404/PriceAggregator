using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.Extensions;
using PriceAggregator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Percentage
{
    public class PercentageHour3 : IPercentage
    {
        private readonly IRepository repository;
        private readonly string simbol;
        private readonly string interval;

        public PercentageHour3(IRepository repository, string simbol, string interval)
        {
            this.repository = repository;
            this.simbol = simbol;
            this.interval = interval;
        }

        public PercentageChange GetPercentage()
        {
            return this.GetPercentageChange(repository, simbol, KlineTimeframe.hour1, interval, 3);
        }
    }
}
