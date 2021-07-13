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
    public class PercentageWeekly2 : IPercentage
    {
        private readonly IRepository repository;
        private readonly string simbol;
        private readonly string interval;

        public PercentageWeekly2(IRepository repository, string simbol, string interval)
        {
            this.repository = repository;
            this.simbol = simbol;
            this.interval = interval;
        }

        public PercentageChange GetPercentage()
        {
            return this.GetPercentageChange(repository, simbol, KlineTimeframe.weekly1, interval, 2);
        }
    }
}
