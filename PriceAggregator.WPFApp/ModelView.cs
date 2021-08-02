using Alerts;
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
        private readonly VolatilityTodayWiewService volatilityTodayWiewService;
        private readonly VolatilityWeeklyViewService volatilityWeeklyViewService;
        private readonly CommonSettings commonSettings;
        private readonly TelegramAlert telegramAlert;

        public ScreenManager ScreenManager { get; private set; }
        public SettingsScreen SettingsScreen { get; private set; }

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
            volatilityTodayWiewService = new VolatilityTodayWiewService();
            volatilityWeeklyViewService = new VolatilityWeeklyViewService();

            PercentageViews = new ObservableCollection<PercentageView>();
            GreenRedPercentViews = new ObservableCollection<GreenRedPercentView>();
            VolatilityTodayViews = new ObservableCollection<VolatilityTodayView>();
            VolatilityWeeklyViews = new ObservableCollection<VolatilityWeeklyView>();
            commonSettings = new CommonSettings();
            telegramAlert = new TelegramAlert(commonSettings, priceAggregatorManager);

            ScreenManager = new ScreenManager();
            SettingsScreen = new SettingsScreen(priceAggregatorManager.Pairs.Where(x => x.Contains("BTC")), //.Take(COUNT_SIMBOLS),
                PercentageViews,
                GreenRedPercentViews,
                VolatilityTodayViews,
                VolatilityWeeklyViews,
                commonSettings); // !!! del Take
        }

        public ObservableCollection<PercentageView> PercentageViews { get; set; }
        public ObservableCollection<GreenRedPercentView> GreenRedPercentViews { get; set; }
        public ObservableCollection<VolatilityTodayView> VolatilityTodayViews { get; set; }
        public ObservableCollection<VolatilityWeeklyView> VolatilityWeeklyViews { get; set; }

        // test
        private const int COUNT_SIMBOLS = 10;

        private async Task calculatingStart()
        {
            var simbols = priceAggregatorManager.Pairs.Where(x => x.Contains("BTC")); //.Take(COUNT_SIMBOLS); // не должно быть из настроек
            var intervals = KlineTimeframe.TimeframesForAggregator;
            await priceAggregatorManager.RunAsync(simbols, intervals).ConfigureAwait(false);
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var simbols = SettingsScreen.SimbolViews.Where(x => x.IsSelected)?.Select(x => x.Simbol);//priceAggregatorManager.Pairs.Take(COUNT_SIMBOLS); // должно быть из настроек

            // percentage
            if (ScreenManager.IsEnabledPecentageScreen == Visibility.Visible)
            {
                var percentViews = await percentageViewsService.CreateViewModels(priceAggregatorManager.PercentageChanges, simbols).ConfigureAwait(false);
                await dispatcher.InvokeAsync(() =>
                {
                    foreach (var percentView in percentViews)
                    {
                        try
                        {
                            var p = PercentageViews.FirstOrDefault(x => x.Simbol == percentView.Simbol);
                            PercentageViews.Remove(p);
                            PercentageViews.Add(percentView);
                        }
                        catch { }
                    }
                });
            }
            else
            {
                await dispatcher.InvokeAsync(() =>
                {
                    PercentageViews.Clear();
                });
            }

            // green/red percentage
            if (ScreenManager.IsEnabledGreenRedScreen == Visibility.Visible)
            {
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
            else
            {
                await dispatcher.InvokeAsync(() =>
                {
                    GreenRedPercentViews.Clear();
                });
            }

            // today volatility
            if (ScreenManager.IsEnabledVolatilityTodayScreen == Visibility.Visible)
            {
                var volatilityTodaylViews = await volatilityTodayWiewService.CreateViewModels(priceAggregatorManager.VolatilityTodayModels, simbols).ConfigureAwait(false);
                await dispatcher.InvokeAsync(() =>
                {
                    foreach (var volatilityModelView in volatilityTodaylViews)
                    {
                        try
                        {
                            var p = VolatilityTodayViews.FirstOrDefault(x => x.Simbol == volatilityModelView.Simbol);
                            VolatilityTodayViews.Remove(p);
                            VolatilityTodayViews.Add(volatilityModelView);
                        }
                        catch { }
                    }
                });
            }
            else
            {
                await dispatcher.InvokeAsync(() =>
                {
                    VolatilityTodayViews.Clear();
                });
            }

            // wekly volatility
            if (ScreenManager.IsEnabledVolatilityWeeklyScreen == Visibility.Visible)
            {
                var volatitlityWeeklyViews = await volatilityWeeklyViewService.CreateViewModels(priceAggregatorManager.VolatilityWeeklyModels, simbols).ConfigureAwait(false);
                await dispatcher.InvokeAsync(() =>
                {
                    foreach (var volatitlityWeeklyView in volatitlityWeeklyViews)
                    {
                        try
                        {
                            var p = VolatilityWeeklyViews.FirstOrDefault(x => x.Simbol == volatitlityWeeklyView.Simbol);
                            VolatilityWeeklyViews.Remove(p);
                            VolatilityWeeklyViews.Add(volatitlityWeeklyView);
                        }
                        catch { }
                    }
                });
            }
            else
            {
                await dispatcher.InvokeAsync(() =>
                {
                    VolatilityWeeklyViews.Clear();
                });
            }
        }

        #region Обработчики событий основного окна
        private async void MainWindow_Initialized(object sender, EventArgs e)
        {
            //MessageBox.Show("Start");
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
