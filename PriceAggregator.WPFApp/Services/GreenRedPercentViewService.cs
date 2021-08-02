using Binance.Common.Kline;
using PriceAggregator.Library.GreenRedPercentage;
using PriceAggregator.WPFApp.Extensions;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.Services
{
    class GreenRedPercentViewService
    {
        public async Task<List<GreenRedPercentView>> CreateViewModels(IEnumerable<GreenRedPercentChange> percentageChanges, IEnumerable<string> simbols)
        {
            var result = new List<GreenRedPercentView>();
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
                                var percentageView = new GreenRedPercentView() { Simbol = simbol };
                                var percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.minute1);
                                percentageView.Percentage1mg = getPercentageGreen(percentItem, out decimal? percentRed);
                                percentageView.Percentage1mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.minute5);
                                percentageView.Percentage5mg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage5mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.minute15);
                                percentageView.Percentage15mg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage15mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.minute30);
                                percentageView.Percentage30mg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage30mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.hour1);
                                percentageView.Percentage1hg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage1hr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.hour3);
                                percentageView.Percentage3hg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage3hr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.hour6);
                                percentageView.Percentage6hg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage6hr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.hour12);
                                percentageView.Percentage12hg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage12hr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.day1);
                                percentageView.Percentage1dg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage1dr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.day2);
                                percentageView.Percentage2dg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage2dr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.day3);
                                percentageView.Percentage3dg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage3dr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.day5);
                                percentageView.Percentage5dg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage5dr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.weekly1);
                                percentageView.Percentage1wg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage1wr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.weekly2);
                                percentageView.Percentage2wg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage2wr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.month1);
                                percentageView.Percentage1Mg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage1Mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.month2);
                                percentageView.Percentage2Mg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage2Mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.month3);
                                percentageView.Percentage3Mg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage3Mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.month6);
                                percentageView.Percentage6Mg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage6Mr = percentRed;
                                percentItem = percentageSimbol.FirstOrDefault(x => x.Interval == KlineTimeframe.year1);
                                percentageView.Percentage1Yg = getPercentageGreen(percentItem, out percentRed);
                                percentageView.Percentage1Yr = percentRed;
                                result.Add(percentageView);
                            }
                        }
                    }
                }
            });
            return result;
        }

        private decimal? getPercentageGreen(GreenRedPercentChange percentItem, out decimal? percentRed)
        {
            percentRed = null;
            if (percentItem != null)
            {
                percentRed = getPercentage(percentItem.PercentageRed);
                return getPercentage(percentItem.PercentageGreen);
            }
            return null;
        }

        private decimal? getPercentage(decimal? percentage)
        {
            if (percentage.HasValue)
            {
                return Math.Round(percentage.Value, 2); //.DecimalToString(2);
            }
            return null;
        }
    }
}
