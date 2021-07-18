using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.GreenRedPercentage;
using PriceAggregator.Library.Interfaces;
using PriceAggregator.Library.Percentage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Extensions
{
    public static class PercentageExtension
    {
        public static decimal? GetPercentage(this IEnumerable<Candle> candles, int countTake)
        {
            try
            {
                if (candles?.Count() > 0)
                {
                    var candlesCloses = candles.Where(x => x.IsClose);
                    if (candlesCloses?.Count() > 0)
                    {
                        var candlesClosesSort = candlesCloses.OrderByDescending(x => x.TimeOpen).Take(countTake);
                        if (candlesClosesSort.Count() == countTake)
                        {
                            var open = candlesClosesSort.Last().Open;
                            var close = candlesClosesSort.First().Close;
                            var percentage = ((close - open) / open) * 100;
                            return percentage;
                        }
                    }
                }
            }
            catch
            {
                // на случай исключения: коллекция была изменена...
                // пока не знаю, как это обойти
            }
            return null;
        }

        /// <summary>
        /// Возвращает PercentageChange
        /// </summary>
        /// <param name="percentageModel"></param>
        /// <param name="repository"></param>
        /// <param name="simbol"></param>
        /// <param name="intervalBinance">Базовый интервал из документации Бинанс</param>
        /// <param name="intervalUser">Интервал пользователя</param>
        /// <param name="countTake"></param>
        /// <returns></returns>
        public static PercentageChange GetPercentageChange(this IPercentage percentageModel, IRepository repository, string simbol, string intervalBinance, string intervalUser, int countTake)
        {
            var candles = repository.Get(simbol, intervalBinance);
            var percentage = candles.GetPercentage(countTake);
            return new PercentageChange()
            {
                Interval = intervalUser,
                Percentage = percentage,
                Simbol = simbol
            };
        }


        public static GreenRedPercentChange GetGreenRedPercentChange(this IGreenRedPercent percentageModel, IRepository repository, string simbol, string intervalBinance, string intervalUser, int countTake)
        {
            var result = new GreenRedPercentChange()
            {
                Simbol = simbol,
                Interval = intervalUser
            };
            var candles = repository.Get(simbol, intervalBinance);
            try
            {
                if (candles?.Count > 0)
                {
                    var candleCloses = candles.OrderByDescending(x => x.Open).Where(x => x.IsClose);
                    var depthCandle = CommonSettings.COUNT_CANDLE_DEPTH * countTake;
                    if (candleCloses?.Count() >= depthCandle)
                    {
                        var candleTakes = candleCloses.Take(depthCandle).ToList();
                        decimal sumGeen = 0;
                        decimal sumRed = 0;

                        for (int i = 0; i < CommonSettings.COUNT_CANDLE_DEPTH; i += countTake)
                        {
                            var open = candleTakes[i].Open;
                            var close = candleTakes[i + countTake - 1].Close;
                            var difference = close - open;
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

                        result.PercentageGreen = GetPercent(sumDiffirence, sumGeen);
                        result.PercentageRed = GetPercent(sumDiffirence, sumRed);
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

        public static decimal? GetPercent(decimal sumDiffirence, decimal sumValue)
        {
            if (sumDiffirence > 0)
            {
                return (sumValue / sumDiffirence) * 100;
            }
            return null;
        }
    }
}
