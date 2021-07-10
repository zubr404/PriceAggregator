using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Common.Kline
{
    /// <summary>
    /// Таймфреймы свечей
    /// </summary>
    public class KlineTimeframe
    {
        public static IReadOnlyCollection<string> TimeframesAll = new ReadOnlyCollection<string>(new List<string>()
        {
            "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h", "1d", "3d", "1w", "1M"
        });
        public static IReadOnlyCollection<string> TimeframesIntraday = new ReadOnlyCollection<string>(new List<string>()
        {
            "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h"
        });
    }
}
