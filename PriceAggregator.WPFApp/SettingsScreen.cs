using Binance.Common.Kline;
using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PriceAggregator.WPFApp
{
    public class SettingsScreen : PropertyChangedBase
    {
        private readonly ObservableCollection<PercentageView> percentageViews;
        private readonly ObservableCollection<GreenRedPercentView> greenRedPercentViews;
        private readonly ObservableCollection<VolatilityTodayView> volatilityTodayViews;
        private readonly ObservableCollection<VolatilityWeeklyView> volatilityWeeklyViews;
        private readonly CommonSettings commonSettings;

        public SettingsScreen(IEnumerable<string> simbols,
            ObservableCollection<PercentageView> percentageViews,
            ObservableCollection<GreenRedPercentView> greenRedPercentViews,
            ObservableCollection<VolatilityTodayView> volatilityTodayViews,
            ObservableCollection<VolatilityWeeklyView> volatilityWeeklyViews,
            CommonSettings commonSettings)
        {
            this.percentageViews = percentageViews;
            this.greenRedPercentViews = greenRedPercentViews;
            this.volatilityTodayViews = volatilityTodayViews;
            this.volatilityWeeklyViews = volatilityWeeklyViews;
            this.commonSettings = commonSettings;
            SimbolViews = new ObservableCollection<SimbolSettingsView>();
            setSimbolsSettings(simbols);
            Intervals = KlineTimeframe.TimeframesAll.ToList();

            BotToken = Properties.Settings.Default.BotToken;
            ChatId = Properties.Settings.Default.ChatId;
        }

        public ObservableCollection<SimbolSettingsView> SimbolViews { get; set; }
        public List<string> Intervals { set; get; }

        #region Properties
        public Visibility IsVisibility
        {
            get { return isVisibility; }
            set
            {
                isVisibility = value;
                base.NotifyPropertyChanged();
            }
        }
        private Visibility isVisibility = Visibility.Collapsed;

        public string SelectedInterval
        {
            get { return selectedInterval; }
            set
            {
                selectedInterval = value;
                base.NotifyPropertyChanged();
            }
        }
        private string selectedInterval;

        public decimal PercentageAlert
        {
            get { return percentageAlert; }
            set
            {
                if (value < -100)
                {
                    percentageAlert = 100;
                    base.NotifyPropertyChanged();
                }
                else if (value > 100000)
                {
                    percentageAlert = 100000;
                    base.NotifyPropertyChanged();
                }
                else
                {
                    percentageAlert = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private decimal percentageAlert;

        public string BotToken
        {
            get { return botToken; }
            set
            {
                botToken = value;
                base.NotifyPropertyChanged();
            }
        }
        private string botToken;

        public string ChatId
        {
            get { return chatId; }
            set
            {
                chatId = value;
                base.NotifyPropertyChanged();
            }
        }
        private string chatId;// = Properties.Settings.Default.ChatId;
        #endregion

        #region Commands
        public RelayCommand SelectAllSimbolsCommand
        {
            get
            {
                return selectAllSimbolsCommand ?? new RelayCommand((object o) =>
                {
                    foreach (var simbolView in SimbolViews)
                    {
                        simbolView.IsSelected = true;
                    }
                });
            }
        }
        private RelayCommand selectAllSimbolsCommand;

        public RelayCommand ClearAllSimbolsCommand
        {
            get
            {
                return clearAllSimbolsCommand ?? new RelayCommand((object o) =>
                {
                    foreach (var simbolView in SimbolViews)
                    {
                        simbolView.IsSelected = false;
                    }
                });
            }
        }
        private RelayCommand clearAllSimbolsCommand;

        public RelayCommand SettingsOpenCommand
        {
            get
            {
                return settingsOpenCommand ?? new RelayCommand((object o) =>
                {
                    IsVisibility = Visibility.Visible;
                });
            }
        }
        private RelayCommand settingsOpenCommand;

        public RelayCommand SettingsCloseCommand
        {
            get
            {
                return settingsCloseCommand ?? new RelayCommand((object o) =>
                {
                    IsVisibility = Visibility.Collapsed;
                    //percentageViews.Clear();
                    greenRedPercentViews.Clear();
                    volatilityTodayViews.Clear();
                    volatilityWeeklyViews.Clear();

                    commonSettings.SetSelectedSimbols(SimbolViews.Where(x => x.IsSelected)?.Select(x => x.Simbol));
                    commonSettings.SelectedInterval = SelectedInterval;
                    commonSettings.SelectedPercentageAlert = PercentageAlert;
                    commonSettings.BotToken = BotToken;
                    commonSettings.ChatId = ChatId;

                    Properties.Settings.Default.BotToken = BotToken;
                    Properties.Settings.Default.ChatId = ChatId;
                    Properties.Settings.Default.Save();
                });
            }
        }
        private RelayCommand settingsCloseCommand;
        #endregion

        #region private methods
        private void setSimbolsSettings(IEnumerable<string> simbols)
        {
            if (simbols?.Count() > 0)
            {
                foreach (var simbol in simbols)
                {
                    SimbolViews.Add(new SimbolSettingsView()
                    {
                        Simbol = simbol,
                        IsSelected = false
                    });
                }
            }
        }
        #endregion
    }
}
