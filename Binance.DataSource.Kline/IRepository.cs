using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.DataSource.Kline
{
    public interface IRepository
    {
        DataOperationType CreateOrUpdate(Candle candle, bool fixedDepthHistory = true);
        Candles Get();
        IReadOnlyCollection<Candle> Get(string simbol, string interval);
    }
}
