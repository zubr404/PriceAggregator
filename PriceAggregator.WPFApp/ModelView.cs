using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PriceAggregator.WPFApp
{
    public class ModelView
    {
        private readonly Dispatcher dispatcher;
        private readonly IRepository candlesRepository;
        private readonly PriceAggregatorManager priceAggregatorManager;

        public ModelView()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            candlesRepository = new CandlesRepository();
            priceAggregatorManager = new PriceAggregatorManager(candlesRepository);

            calculatingStart();
        }

        public List<PercentageView> PercentageViews { get; set; }

        private async Task calculatingStart()
        {
            var simbols = priceAggregatorManager.Pairs;
            var intervals = KlineTimeframe.TimeframesForAggregator;
            await priceAggregatorManager.RunAsync(simbols, intervals);
        }
    }
}
