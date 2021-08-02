using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.ViewModels
{
    public class VolatilityTodayView
    {
        public string Simbol { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? CurrentPrice { get; set; }
        /// <summary>
        /// Текущая цена относительно максимума %
        /// </summary>
        public decimal? PercentagePriceHigh { get; set; }
        /// <summary>
        /// Текущая цена относительно минимума %
        /// </summary>
        public decimal? PercentagePriceLow { get; set; }
    }
}
