using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Common.Kline
{
    /// <summary>
    /// Общие для всего проекта настройки
    /// </summary>
    public class CommonSettings
    {
        public const int LIMIT_KLINES = 100;
        public static readonly TimeSpan RESTART_STREAM_TIME = new TimeSpan(2, 27, 00);
        public static readonly TimeSpan INTERVAL_CHANNEL_RESTART = new TimeSpan(0, 0, 5);
        public const int COUNT_CANDLE_DEPTH = 6;
    }
}
