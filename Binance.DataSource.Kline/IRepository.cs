using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.DataSource.Kline
{
    public interface IRepository
    {
        DataOperationType CreateOrUpdate(Candle candle);
        Candles Get();
        IEnumerable<Candle> Get(string simbol, string interval);
        void Remove(string simbol, string interval, long timeOpen);
        void RemoveFirst(string simbol, string interval);
    }
}
