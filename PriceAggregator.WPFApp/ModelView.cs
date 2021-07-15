using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            Application.Current.MainWindow.Closed += MainWindow_Closed;
            Application.Current.MainWindow.Initialized += MainWindow_Initialized;
        }

        public List<PercentageView> PercentageViews { get; set; }

        private async Task calculatingStart()
        {
            var simbols = priceAggregatorManager.Pairs;
            var intervals = KlineTimeframe.TimeframesForAggregator;
            await priceAggregatorManager.RunAsync(simbols, intervals);
        }

        #region Обработчики событий основного окна
        private async void MainWindow_Initialized(object sender, EventArgs e)
        {
            await calculatingStart();
            MessageBox.Show("Loading finish");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            priceAggregatorManager.ThreadAbort();
        }
        #endregion
    }
}
