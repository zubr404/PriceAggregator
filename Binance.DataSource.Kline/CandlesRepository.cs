using Binance.Common.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.DataSource.Kline
{
    public class CandlesRepository : IRepository
    {
        private readonly DataContext dataContext;
        private readonly object locker;
        public CandlesRepository()
        {
            dataContext = new DataContext();
            locker = new object();
        }
        public DataOperationType CreateOrUpdate(Candle candle, bool fixedDepthHistory = true)
        {
            lock (locker)
            {
                if (dataContext.Candles.ContainsKey(candle.Simbol))
                {
                    if (dataContext.Candles[candle.Simbol].ContainsKey(candle.Interval))
                    {
                        var candles = dataContext.Candles[candle.Simbol][candle.Interval];
                        var candleFind = candles.FirstOrDefault(x => x.TimeOpen == candle.TimeOpen);
                        if (candleFind != null) // update
                        {
                            candleFind.Close = candle.Close;
                            candleFind.High = candle.High;
                            candleFind.IsClose = candle.IsClose;
                            candleFind.Low = candle.Low;
                            candleFind.TimeClose = candle.TimeClose;
                            return DataOperationType.Update;
                        }
                        else
                        {
                            candles.Add(candle);
                            if (fixedDepthHistory)
                            {
                                if (candles.Count > CommonSettings.LIMIT_KLINES) // поддерживаем определенную глубину истории
                                {
                                    var minOpenTime = candles.Min(x => x.TimeOpen);
                                    var firstCandleIndex = candles.FindIndex(x => x.TimeOpen == minOpenTime);
                                    candles.RemoveAt(firstCandleIndex);
                                }
                            }
                            return DataOperationType.Add;
                        }
                    }
                    else
                    {
                        dataContext.Candles[candle.Simbol].Add(candle.Interval, new List<Candle>() { candle });
                        return DataOperationType.Add;
                    }
                }
                else
                {
                    dataContext.Candles.Add(candle.Simbol, new Dictionary<string, List<Candle>>
                    {
                        {candle.Interval, new List<Candle>() { candle } }
                    });
                    return DataOperationType.Add;
                }
            }
        }

        public Candles Get()
        {
            return dataContext.Candles;
        }

        public IReadOnlyCollection<Candle> Get(string simbol, string interval)
        {
            if (dataContext.Candles.ContainsKey(simbol))
            {
                if (dataContext.Candles[simbol].ContainsKey(interval))
                {
                    return dataContext.Candles[simbol][interval];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
