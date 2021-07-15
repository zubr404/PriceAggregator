using Binance.Common.Kline;
using PriceAggregator.Library;
using PriceAggregator.Library.Percentage;
using PriceAggregator.WPFApp.Extensions;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.Services
{
    class PercentageViewsService
    {
        private readonly IEnumerable<PercentageChange> percentageChanges;

        public PercentageViewsService(IEnumerable<PercentageChange> percentageChanges)
        {
            this.percentageChanges = percentageChanges;
        }

        public List<PercentageView> CreateViewModels(IEnumerable<string> simbols)
        {
            if (simbols?.Count() > 0)
            {
                if (percentageChanges?.Count() > 0)
                {
                    var result = new List<PercentageView>(simbols.Count());
                    foreach (var simbol in simbols)
                    {
                        var percentageSimbol = percentageChanges.Where(x => x.Simbol == simbol);
                        if (percentageSimbol?.Count() > 0)
                        {
                            var percentageView = new PercentageView()
                            {
                                Simbol = simbol,
                                Percentage1m = getPercentage(percentageSimbol, KlineTimeframe.minute1)
                            };
                        }
                    }
                }
            }
            return new List<PercentageView>(0);
        }

        private string getPercentage(IEnumerable<PercentageChange> percentageChanges, string interval)
        {
            var percentageChange = percentageChanges.FirstOrDefault(x => x.Interval == interval);
            if (percentageChange != null)
            {
                if (percentageChange.Percentage.HasValue)
                {
                    return percentageChange.Percentage.Value.DecimalToString(2);
                }
            }
            return null;
        }
    }
}
