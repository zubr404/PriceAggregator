using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.DataSource.Kline
{
    public class Candle
    {
        /// <summary>
        /// Время открытия свечи (UNIXTIME)
        /// </summary>
        public long TimeOpen { get; set; }
        /// <summary>
        /// Время закрытия свечи
        /// </summary>
        public long TimeClose { get; set; }
        /// <summary>
        /// Пара
        /// </summary>
        public string Simbol { get; set; }
        /// <summary>
        /// Период
        /// </summary>
        public string Interval { get; set; }
        /// <summary>
        /// Цена открытия
        /// </summary>
        public decimal Open { get; set; }
        /// <summary>
        /// Цена закрытия
        /// </summary>
        public decimal Close { get; set; }
        /// <summary>
        /// High
        /// </summary>
        public decimal High { get; set; }
        /// <summary>
        /// Low
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// Закрыта ли свеча?
        /// </summary>
        public bool IsClose { get; set; }
    }
}
