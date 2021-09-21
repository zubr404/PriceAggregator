using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ChartHistoryLoader
{
    public static class UnixTimeConverter
    {
        public static DateTime UnixToDateTime(this long timestamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddMilliseconds(Math.Abs(timestamp));
        }

        public static long ToUnixTime(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
