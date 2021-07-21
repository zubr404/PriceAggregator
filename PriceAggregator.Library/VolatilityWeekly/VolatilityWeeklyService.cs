using Binance.Common.Kline;
using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.VolatilityWeekly
{
    public class VolatilityWeeklyService
    {
        private readonly IRepository repository;

        public VolatilityWeeklyService(IRepository repository)
        {
            this.repository = repository;
        }

        public List<VolatilityWeeklyModel> GetVolatilites(IEnumerable<string> simbols)
        {
            var result = new List<VolatilityWeeklyModel>();
            if (simbols?.Count() > 0)
            {
                foreach (var simbol in simbols)
                {
                    var candles = repository.Get(simbol, KlineTimeframe.day1);
                    if (candles?.Count > 0)
                    {
                        var candleCloses = candles.Where(x => x.IsClose);
                        if (candleCloses?.Count() > 0)
                        {
                            var volatilityWeekly = new VolatilityWeeklyModel(simbol);
                            var weekCandles = candleCloses.OrderByDescending(x => x.TimeOpen).Take(7);
                            for (int i = weekCandles.Count() - 1; i >= 0; i--)
                            {
                                var candle = weekCandles.ElementAt(i);
                                var volatilityDay = new VolatilityDayModel(candle.High, candle.Low);
                                volatilityWeekly.VolatilityDayModels.Add(volatilityDay);
                            }
                            volatilityWeekly.SetResults();
                            result.Add(volatilityWeekly);
                        }
                    }
                }
            }
            return result;
        }
    }
}
