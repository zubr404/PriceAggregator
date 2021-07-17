using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library;
using PriceAggregator.WPFApp.Services;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace PriceAggregator.WPFApp
{
    public class ModelView : PropertyChangedBase
    {
        private readonly Dispatcher dispatcher;
        private readonly IRepository candlesRepository;
        private readonly PriceAggregatorManager priceAggregatorManager;

        private readonly Timer timer;

        private readonly PercentageViewsService percentageViewsService;

        public ModelView()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            candlesRepository = new CandlesRepository();
            priceAggregatorManager = new PriceAggregatorManager(candlesRepository);            

            Application.Current.MainWindow.Closed += MainWindow_Closed;
            Application.Current.MainWindow.Initialized += MainWindow_Initialized;

            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;

            percentageViewsService = new PercentageViewsService();
        }

        public List<PercentageView> PercentageViews
        {
            get { return percentageViews; }
            set
            {
                percentageViews = value;
                base.NotifyPropertyChanged();
            }
        }
        private List<PercentageView> percentageViews;

        private async Task calculatingStart()
        {
            await dispatcher.InvokeAsync(async () =>
            {
                var simbols = priceAggregatorManager.Pairs.Take(100); // не должно быть из настроек
                var intervals = KlineTimeframe.TimeframesForAggregator;
                await priceAggregatorManager.RunAsync(simbols, intervals);
            });
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await dispatcher.InvokeAsync(async () =>
            {
                var simbols = priceAggregatorManager.Pairs.Take(100); // должно быть из настроек
                PercentageViews = await percentageViewsService.CreateViewModels(priceAggregatorManager.PercentageChanges, simbols);
            });
        }

        #region Обработчики событий основного окна
        private async void MainWindow_Initialized(object sender, EventArgs e)
        {
            await calculatingStart();
            timer.Start();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            priceAggregatorManager.ThreadAbort();
        }
        #endregion
    }
}
