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

        public SettingsScreen(IEnumerable<string> simbols,
            ObservableCollection<PercentageView> percentageViews,
            ObservableCollection<GreenRedPercentView> greenRedPercentViews,
            ObservableCollection<VolatilityTodayView> volatilityTodayViews,
            ObservableCollection<VolatilityWeeklyView> volatilityWeeklyViews)
        {
            this.percentageViews = percentageViews;
            this.greenRedPercentViews = greenRedPercentViews;
            this.volatilityTodayViews = volatilityTodayViews;
            this.volatilityWeeklyViews = volatilityWeeklyViews;
            SimbolViews = new ObservableCollection<SimbolView>();
            setSimbolsSettings(simbols);
        }

        private void setSimbolsSettings(IEnumerable<string> simbols)
        {
            if (simbols?.Count() > 0)
            {
                foreach (var simbol in simbols)
                {
                    SimbolViews.Add(new SimbolView()
                    {
                        Simbol = simbol,
                        IsSelected = false
                    });
                }
            }
        }

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

        public ObservableCollection<SimbolView> SimbolViews { get; set; }

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
                    percentageViews.Clear();
                    greenRedPercentViews.Clear();
                    volatilityTodayViews.Clear();
                    volatilityWeeklyViews.Clear();
                });
            }
        }
        private RelayCommand settingsCloseCommand;
    }
}
