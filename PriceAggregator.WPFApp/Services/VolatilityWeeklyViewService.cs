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
                                            volatilityView.HighDay1 = volatilityDay.High.DecimalToString(15);
                                            volatilityView.LowDay1 = volatilityDay.Low.DecimalToString(15);
                                            volatilityView.PercentageDay1 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 1:
                                            volatilityView.HighDay2 = volatilityDay.High.DecimalToString(15);
                                            volatilityView.LowDay2 = volatilityDay.Low.DecimalToString(15);
                                            volatilityView.PercentageDay2 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 2:
                                            volatilityView.HighDay3 = volatilityDay.High.DecimalToString(15);
                                            volatilityView.LowDay3 = volatilityDay.Low.DecimalToString(15);
                                            volatilityView.PercentageDay3 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 3:
                                            volatilityView.HighDay4 = volatilityDay.High.DecimalToString(15);
                                            volatilityView.LowDay4 = volatilityDay.Low.DecimalToString(15);
                                            volatilityView.PercentageDay4 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 4:
                                            volatilityView.HighDay5 = volatilityDay.High.DecimalToString(15);
                                            volatilityView.LowDay5 = volatilityDay.Low.DecimalToString(15);
                                            volatilityView.PercentageDay5 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 5:
                                            volatilityView.HighDay6 = volatilityDay.High.DecimalToString(15);
                                            volatilityView.LowDay6 = volatilityDay.Low.DecimalToString(15);
                                            volatilityView.PercentageDay6 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        case 6:
                                            volatilityView.HighDay7 = volatilityDay.High.DecimalToString(15);
                                            volatilityView.LowDay7 = volatilityDay.Low.DecimalToString(15);
                                            volatilityView.PercentageDay7 = getPercentage(volatilityDay.Percentage);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                volatilityView.HighWeekly = volatilitySimbol.High.DecimalToString(15);
                                volatilityView.LowWeekly = volatilitySimbol.Low.DecimalToString(15);
                                volatilityView.PercentageWeekly = getPercentage(volatilitySimbol.Percentage);
                                result.Add(volatilityView);
                            }
                        }
                    }
                }
            });
            return result;
        }

        private string getPercentage(decimal? percentage)
        {
            if (percentage.HasValue)
            {
                return percentage.Value.DecimalToString(2);
            }
            return null;
        }
    }
}
