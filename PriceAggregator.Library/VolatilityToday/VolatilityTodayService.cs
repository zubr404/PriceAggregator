using Binance.Common.Kline;
using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.VolatilityToday
{
    public class VolatilityTodayService
    {
        private readonly IRepository repository;

        public VolatilityTodayService(IRepository repository)
        {
            this.repository = repository;
        }

        public List<VolatilityTodayModel> GetVolatilites(IEnumerable<string> simbols)
        {
            var result = new List<VolatilityTodayModel>();
            if (simbols?.Count() > 0)
            {
                foreach (var simbol in simbols)
                {
                    var candles = repository.Get(simbol, KlineTimeframe.day1);
                    if (candles?.Count > 0)
                    {
                        var candle = candles.FirstOrDefault(x => !x.IsClose);
                        if (candle != null)
                        {
                            var volatility = new VolatilityTodayModel
                            {
                                Simbol = simbol,
                                High = candle.High,
                                Low = candle.Low,
                                CurrentPrice = candle.Close,
                                PercentagePriceHigh = getPercentage(candle.High, candle.Close),
                                PercentagePriceLow = getPercentage(candle.Low, candle.Close)
                            };
                            result.Add(volatility);
                        }
                    }
                }
            }
            return result;
        }

        private decimal? getPercentage(decimal value1, decimal value2)
        {
            var difference = value1 - value2;
            if (value1 > 0)
            {
                return (difference / value1) * 100;
            }
            return null;
        }
    }
}
