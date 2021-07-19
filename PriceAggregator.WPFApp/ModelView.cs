using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library;
using PriceAggregator.WPFApp.Services;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        private readonly System.Timers.Timer timer;

        private readonly PercentageViewsService percentageViewsService;
        private readonly GreenRedPercentViewService greenRedPercentViewService;

        public ModelView()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            candlesRepository = new CandlesRepository();
            priceAggregatorManager = new PriceAggregatorManager(candlesRepository);            

            Application.Current.MainWindow.Closed += MainWindow_Closed;
            Application.Current.MainWindow.Initialized += MainWindow_Initialized;

            timer = new System.Timers.Timer(5000);
            timer.Elapsed += Timer_Elapsed;

            percentageViewsService = new PercentageViewsService();
            greenRedPercentViewService = new GreenRedPercentViewService();
            PercentageViews = new ObservableCollection<PercentageView>();
            GreenRedPercentViews = new ObservableCollection<GreenRedPercentView>();
        }

        public ObservableCollection<PercentageView> PercentageViews { get; set; }
        public ObservableCollection<GreenRedPercentView> GreenRedPercentViews { get; set; }

        // test
        private const int countSimbols = 10;

        private async Task calculatingStart()
        {
            var simbols = priceAggregatorManager.Pairs.Take(countSimbols); // не должно быть из настроек
            var intervals = KlineTimeframe.TimeframesForAggregator;
            await priceAggregatorManager.RunAsync(simbols, intervals).ConfigureAwait(false);
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            var simbols = priceAggregatorManager.Pairs.Take(countSimbols); // должно быть из настроек

            // percentage
            //var percentViews = await percentageViewsService.CreateViewModels(priceAggregatorManager.PercentageChanges, simbols).ConfigureAwait(false);
            //await dispatcher.InvokeAsync(() =>
            //{
            //    foreach (var percentView in percentViews)
            //    {
            //        try
            //        {
            //            var p = PercentageViews.FirstOrDefault(x => x.Simbol == percentView.Simbol);
            //            PercentageViews.Remove(p);
            //            PercentageViews.Add(percentView);
            //        }
            //        catch { }
            //    }
            //});

            // green/red percentage
            var greenRedPercentViews = await greenRedPercentViewService.CreateViewModels(priceAggregatorManager.GreenRedPercentChanges, simbols).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                foreach (var greenRedPercentView in greenRedPercentViews)
                {
                    try
                    {
                        var p = GreenRedPercentViews.FirstOrDefault(x => x.Simbol == greenRedPercentView.Simbol);
                        GreenRedPercentViews.Remove(p);
                        GreenRedPercentViews.Add(greenRedPercentView);
                    }
                    catch { }
                }
            });
        }

        #region Обработчики событий основного окна
        private async void MainWindow_Initialized(object sender, EventArgs e)
        {
            MessageBox.Show("Start");
            await Task.Run(async () =>
            {
                await calculatingStart().ConfigureAwait(false);
                timer.Start();
            });
            //MessageBox.Show("Finish");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            priceAggregatorManager.ThreadAbort();
        }
        #endregion
    }
}
