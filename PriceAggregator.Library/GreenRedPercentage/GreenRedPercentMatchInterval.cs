using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.Extensions;
using PriceAggregator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.GreenRedPercentage
{
    /// <summary>
    /// Вычисление красных/зеленых процентов для таймфреймов, которые совпадают с документацией Бинанс
    /// </summary>
    public class GreenRedPercentMatchInterval : IGreenRedPercent
    {
        private readonly IRepository repository;
        private readonly string simbol;
        private readonly string interval;

        public GreenRedPercentMatchInterval(IRepository repository, string simbol, string interval)
        {
            this.repository = repository;
            this.simbol = simbol;
            this.interval = interval;
        }

        public GreenRedPercentChange GetPercentage()
        {
            var result = new GreenRedPercentChange()
            {
                Simbol = simbol,
                Interval = interval
            };
            var candles = repository.Get(simbol, interval);
            try
            {
                if (candles?.Count > 0)
                {
                    var candleCloses = candles.OrderByDescending(x => x.Open).Where(x => x.IsClose);
                    if (candleCloses?.Count() >= CommonSettings.COUNT_CANDLE_DEPTH)
                    {
                        var candleTakes = candleCloses.Take(CommonSettings.COUNT_CANDLE_DEPTH);
                        decimal sumGeen = 0;
                        decimal sumRed = 0;
                        foreach (var candleTake in candleTakes)
                        {
                            var difference = candleTake.Close - candleTake.Open;
                            if (difference > 0)
                            {
                                sumGeen += difference;
                            }
                            else
                            {
                                sumRed += Math.Abs(difference);
                            }
                        }
                        var sumDiffirence = sumGeen + sumRed;

                        result.PercentageGreen = PercentageExtension.GetPercent(sumDiffirence, sumGeen);
                        result.PercentageRed = PercentageExtension.GetPercent(sumDiffirence, sumRed);
                    }
                }
            }
            catch
            {
                // на случай исключения: коллекция была изменена...
                // пока не знаю, как это обойти
            }
            return result;
        }
    }
}
