using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.DataSource.Kline
{
    class DataContext
    {
        public Candles Candles { get; private set; }

        public DataContext()
        {
            Candles = new Candles();
        }
    }
}
