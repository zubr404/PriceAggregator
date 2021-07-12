using Binance.DataSource.Kline;
using PriceAggregator.Library.Percentage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library
{
    public class PriceAggregatorManager
    {
        private readonly PercentageChangeService percentageChangeService;

        public PriceAggregatorManager(IRepository repository)
        {
            percentageChangeService = new PercentageChangeService(repository);
        }

        public List<PercentageChange> PercentageChanges { get; private set; }

        public void Run(IEnumerable<string> simbols, IEnumerable<string> intervals)
        {
            PercentageChanges = percentageChangeService.GetPercentages(simbols, intervals);
        }
    }
}
