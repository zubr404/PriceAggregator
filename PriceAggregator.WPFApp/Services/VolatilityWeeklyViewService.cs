using PriceAggregator.Library.VolatilityWeekly;
using PriceAggregator.WPFApp.Extensions;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.Services
{
    class VolatilityWeeklyViewService
    {
        public async Task<List<VolatilityWeeklyView>> CreateViewModels(IEnumerable<VolatilityWeeklyModel> volatilityWeeklyModels, IEnumerable<string> simbols)
        {
            var result = new List<VolatilityWeeklyView>();
            await Task.Run(() =>
            {
                if (simbols?.Count() > 0)
                {
                    if (volatilityWeeklyModels?.Count() > 0)
                    {
                        foreach (var simbol in simbols)
                        {
                            var volatilitySimbol = volatilityWeeklyModels.FirstOrDefault(x => x.Simbol == simbol);
                            if (volatilitySimbol != null)
                            {
                                var volatilityView = new VolatilityWeeklyView();
                                volatilityView.Simbol = simbol;
                                for (int i = 0; i < volatilitySimbol.VolatilityDayModels.Count; i++)
                                {
                                    var volatilityDay = volatilitySimbol.VolatilityDayModels[i];
                                    switch (i)
                                    {
                                        case 0:
                                            volatilityView.HighDay1 = Math.Round(volatilityDay.High, 15);
                                            volatilityView.LowDay1 = Math.Round(volatilityDay.Low, 15);
                                            volatilityView.PercentageDay1 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 1:
                                            volatilityView.HighDay2 = Math.Round(volatilityDay.High, 15);
                                            volatilityView.LowDay2 = Math.Round(volatilityDay.Low, 15);
                                            volatilityView.PercentageDay2 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 2:
                                            volatilityView.HighDay3 = Math.Round(volatilityDay.High, 15);
                                            volatilityView.LowDay3 = Math.Round(volatilityDay.Low, 15);
                                            volatilityView.PercentageDay3 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 3:
                                            volatilityView.HighDay4 = Math.Round(volatilityDay.High, 15);
                                            volatilityView.LowDay4 = Math.Round(volatilityDay.Low, 15);
                                            volatilityView.PercentageDay4 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 4:
                                            volatilityView.HighDay5 = Math.Round(volatilityDay.High, 15);
                                            volatilityView.LowDay5 = Math.Round(volatilityDay.Low, 15);
                                            volatilityView.PercentageDay5 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 5:
                                            volatilityView.HighDay6 = Math.Round(volatilityDay.High, 15);
                                            volatilityView.LowDay6 = Math.Round(volatilityDay.Low, 15);
                                            volatilityView.PercentageDay6 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 6:
                                            volatilityView.HighDay7 = Math.Round(volatilityDay.High, 15);
                                            volatilityView.LowDay7 = Math.Round(volatilityDay.Low, 15);
                                            volatilityView.PercentageDay7 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                volatilityView.HighWeekly = Math.Round(volatilitySimbol.High, 2); //.DecimalToString(15);
                                volatilityView.LowWeekly = Math.Round(volatilitySimbol.Low, 2); //.DecimalToString(15);
                                volatilityView.PercentageWeekly = getPercentage(volatilitySimbol.Percentage);
                                result.Add(volatilityView);
                            }
                        }
                    }
                }
            });
            return result;
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
