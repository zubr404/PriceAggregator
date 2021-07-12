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
    class PercentageFactory
    {
        private readonly IRepository repository;
        private readonly IEnumerable<string> simbols;
        private readonly IEnumerable<string> intervals;

        public PercentageFactory(IRepository repository, IEnumerable<string> simbols, IEnumerable<string> intervals)
        {
            this.repository = repository;
            this.simbols = simbols;
            this.intervals = intervals;
        }

        public IList<IPercentage> GetPercentageModls()
        {
            var result = new List<IPercentage>();
            if (simbols?.Count() > 0)
            {
                if (intervals?.Count() > 0)
                {
                    foreach (var simbol in simbols)
                    {
                        foreach (var interval in intervals)
                        {
                            if (KlineTimeframe.TimeframesAdaptive.Contains(interval)) // если интервал совпадает с Бинанс
                            {
                                result.Add(new PercentageMatchingInterval(repository, simbol, interval));
                            }
                            else
                            {
                                switch (interval)
                                {
                                    case KlineTimeframe.hour3:
                                        result.Add(new PercentageHour3(repository, simbol, interval));
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
