using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.DataSource.Kline
{
    /// <summary>
    /// Dictionary<Simbol, Dictionary<Interval, List<Candle>>>
    /// </summary>
    public class Candles : Dictionary<string, Dictionary<string, List<Candle>>>
    {
    }
}
