using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.Extensions;
using PriceAggregator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.GreenRedPercentage
{
    public class GreenRedPercentMonth6 : IGreenRedPercent
	{
		private readonly IRepository repository;
		private readonly string simbol;
		private readonly string interval;

		public GreenRedPercentMonth6(IRepository repository, string simbol, string interval)
		{
			this.repository = repository;
			this.simbol = simbol;
			this.interval = interval;
		}

		public GreenRedPercentChange GetPercentage()
		{
			return this.GetGreenRedPercentChange(repository, simbol, KlineTimeframe.month1, interval, 6);
		}
	}
}
