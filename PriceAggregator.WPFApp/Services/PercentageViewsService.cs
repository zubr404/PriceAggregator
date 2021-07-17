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
        public async Task<List<PercentageView>> CreateViewModels(IEnumerable<PercentageChange> percentageChanges, IEnumerable<string> simbols)
        {
            var result = new List<PercentageView>();
            await Task.Run(() =>
            {
                if (simbols?.Count() > 0)
                {
                    if (percentageChanges?.Count() > 0)
                    {
                        foreach (var simbol in simbols)
                        {
                            var percentageSimbol = percentageChanges.Where(x => x.Simbol == simbol);
                            if (percentageSimbol?.Count() > 0)
                            {
                                var percentageView = new PercentageView()
                                {
                                    Simbol = simbol,
                                    Percentage1m = getPercentage(percentageSimbol, KlineTimeframe.minute1),
                                    Percentage5m = getPercentage(percentageSimbol, KlineTimeframe.minute5),
                                    Percentage15m = getPercentage(percentageSimbol, KlineTimeframe.minute15),
                                    Percentage30m = getPercentage(percentageSimbol, KlineTimeframe.minute30),
                                    Percentage1h = getPercentage(percentageSimbol, KlineTimeframe.hour1),
                                    Percentage3h = getPercentage(percentageSimbol, KlineTimeframe.hour3),
                                    Percentage6h = getPercentage(percentageSimbol, KlineTimeframe.hour6),
                                    Percentage12h = getPercentage(percentageSimbol, KlineTimeframe.hour12),
                                    Percentage1d = getPercentage(percentageSimbol, KlineTimeframe.day1),
                                    Percentage2d = getPercentage(percentageSimbol, KlineTimeframe.day2),
                                    Percentage3d = getPercentage(percentageSimbol, KlineTimeframe.day3),
                                    Percentage5d = getPercentage(percentageSimbol, KlineTimeframe.day5),
                                    Percentage1w = getPercentage(percentageSimbol, KlineTimeframe.weekly1),
                                    Percentage2w = getPercentage(percentageSimbol, KlineTimeframe.weekly2),
                                    Percentage1M = getPercentage(percentageSimbol, KlineTimeframe.month1),
                                    Percentage2M = getPercentage(percentageSimbol, KlineTimeframe.month2),
                                    Percentage3M = getPercentage(percentageSimbol, KlineTimeframe.month3),
                                    Percentage6M = getPercentage(percentageSimbol, KlineTimeframe.month6),
                                    Percentage1Y = getPercentage(percentageSimbol, KlineTimeframe.year1)
                                };
                                result.Add(percentageView);
                            }
                        }
                    }
                }
            });
            return result;
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
