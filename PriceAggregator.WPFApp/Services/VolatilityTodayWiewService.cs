using PriceAggregator.Library.VolatilityToday;
using PriceAggregator.WPFApp.Extensions;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.Services
{
    class VolatilityTodayWiewService
    {
        public async Task<List<VolatilityTodayView>> CreateViewModels(IEnumerable<VolatilityTodayModel> volatilityModels, IEnumerable<string> simbols)
        {
            var result = new List<VolatilityTodayView>();
            await Task.Run(() =>
            {
                if (simbols?.Count() > 0)
                {
                    if (volatilityModels?.Count() > 0)
                    {
                        foreach (var simbol in simbols)
                        {
                            var volatilityModel = volatilityModels.FirstOrDefault(x => x.Simbol == simbol);
                            if (volatilityModel != null)
                            {
                                result.Add(new VolatilityTodayView()
                                {
                                    Simbol = volatilityModel.Simbol,
                                    High = getPercentage(volatilityModel.High, 15),
                                    Low = getPercentage(volatilityModel.Low, 15),
                                    CurrentPrice = getPercentage(volatilityModel.CurrentPrice, 15),
                                    PercentagePriceHigh = getPercentage(volatilityModel.PercentagePriceHigh),
                                    PercentagePriceLow = getPercentage(volatilityModel.PercentagePriceLow)
                                });
                            }
                        }
                    }
                }
            });
            return result;
        }

        private decimal? getPercentage(decimal? percentage, int digits = 2)
        {
            if (percentage.HasValue)
            {
                return Math.Round(percentage.Value, digits); //.DecimalToString(digits);
            }
            return null;
        }
    }
}
