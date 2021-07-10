using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.StockExchange.Kline.REST
{
    public class MarketPair
    {
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public string Pair { get; set; }
        public string Status { get; set; } // "status": "TRADING"
    }
}
