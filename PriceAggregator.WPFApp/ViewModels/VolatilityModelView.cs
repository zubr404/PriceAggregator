using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.ViewModels
{
    public class VolatilityModelView
    {
        public string Simbol { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string CurrentPrice { get; set; }
        /// <summary>
        /// Текущая цена относительно максимума %
        /// </summary>
        public string PercentagePriceHigh { get; set; }
        /// <summary>
        /// Текущая цена относительно минимума %
        /// </summary>
        public string PercentagePriceLow { get; set; }
    }
}
