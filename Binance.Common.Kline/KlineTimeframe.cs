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
        /// <summary>
        /// Интервалы в соответствии с документацией Бинансе
        /// </summary>
        public static IReadOnlyCollection<string> TimeframesAll = new ReadOnlyCollection<string>(new List<string>()
        {
            "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h", "1d", "3d", "1w", "1M"
        });
        public static IReadOnlyCollection<string> TimeframesIntraday = new ReadOnlyCollection<string>(new List<string>()
        {
            "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h"
        });

        /// <summary>
        /// Интервалы в соответствии с документацией Бинансе (без лишних для агрегатора)
        /// </summary>
        public static IReadOnlyCollection<string> TimeframesAdaptive = new ReadOnlyCollection<string>(new List<string>()
        {
            "1m", "5m", "15m", "30m", "1h", "6h", "12h", "1d", "3d", "1w", "1M"
        });

        /// <summary>
        /// Интервалы, укаханнве закзчиком
        /// </summary>
        public static IReadOnlyCollection<string> TimeframesForAggregator = new ReadOnlyCollection<string>(new List<string>()
        {
            "1m", "5m", "15m", "30m", "1h", "3h", "6h", "12h", "1d", "2d", "3d", "5d", "1w", "2w", "1M", "2M", "3M", "6M", "1Y"
        });

        public const string minute1 = "1m";
        public const string minute5 = "5m";
        public const string minute15 = "15m";
        public const string minute30 = "30m";
        public const string hour1 = "1h";
        public const string hour3 = "3h";
        public const string hour6 = "6h";
        public const string hour12 = "12h";
        public const string day1 = "1d";
        public const string day2 = "2d";
        public const string day3 = "3d";
        public const string day5 = "5d";
        public const string weekly1 = "1w";
        public const string weekly2 = "2w";
        public const string month1 = "1M";
        public const string month2 = "2M";
        public const string month3 = "3M";
        public const string month6 = "6M";
        public const string year1 = "1Y";
    }
}
