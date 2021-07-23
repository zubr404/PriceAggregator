using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PriceAggregator.WPFApp
{
    public class ScreenManager : PropertyChangedBase
    {
        public Visibility IsEnabledPecentageScreen
        {
            get { return isEnabledPecentageScreen; }
            set
            {
                isEnabledPecentageScreen = value;
                base.NotifyPropertyChanged();
            }
        }
        private Visibility isEnabledPecentageScreen = Visibility.Visible;

        public Visibility IsEnabledGreenRedScreen
        {
            get { return isEnabledGreenRedScreen; }
            set
            {
                isEnabledGreenRedScreen = value;
                base.NotifyPropertyChanged();
            }
        }
        private Visibility isEnabledGreenRedScreen = Visibility.Collapsed;

        public Visibility IsEnabledVolatilityTodayScreen
        {
            get { return isEnabledVolatilityTodayScreen; }
            set
            {
                isEnabledVolatilityTodayScreen = value;
                base.NotifyPropertyChanged();
            }
        }
        private Visibility isEnabledVolatilityTodayScreen = Visibility.Collapsed;

        public Visibility IsEnabledVolatilityWeeklyScreen
        {
            get { return isEnabledVolatilityWeeklyScreen; }
            set
            {
                isEnabledVolatilityWeeklyScreen = value;
                base.NotifyPropertyChanged();
            }
        }
        private Visibility isEnabledVolatilityWeeklyScreen = Visibility.Collapsed;

        public RelayCommand PercentageScreenOpen
        {
            get
            {
                return percentageScreenOpen ?? new RelayCommand((object o) =>
                {
                    IsEnabledPecentageScreen = Visibility.Visible;
                    IsEnabledGreenRedScreen = Visibility.Collapsed;
                    IsEnabledVolatilityTodayScreen = Visibility.Collapsed;
                    IsEnabledVolatilityWeeklyScreen = Visibility.Collapsed;
                });
            }
        }
        private RelayCommand percentageScreenOpen;

        public RelayCommand GreenRedScreenOpen
        {
            get
            {
                return greenRedScreenOpen ?? new RelayCommand((object o) =>
                {
                    IsEnabledPecentageScreen = Visibility.Collapsed;
                    IsEnabledGreenRedScreen = Visibility.Visible;
                    IsEnabledVolatilityTodayScreen = Visibility.Collapsed;
                    IsEnabledVolatilityWeeklyScreen = Visibility.Collapsed;
                });
            }
        }
        private RelayCommand greenRedScreenOpen;

        public RelayCommand VolatilityTodayScreenOpen
        {
            get
            {
                return volatilityTodayScreenOpen ?? new RelayCommand((object o) =>
                {
                    IsEnabledPecentageScreen = Visibility.Collapsed;
                    IsEnabledGreenRedScreen = Visibility.Collapsed;
                    IsEnabledVolatilityTodayScreen = Visibility.Visible;
                    IsEnabledVolatilityWeeklyScreen = Visibility.Collapsed;
                });
            }
        }
        private RelayCommand volatilityTodayScreenOpen;

        public RelayCommand VolatilityWeeklyScreenOpen
        {
            get
            {
                return volatilityWeeklyScreenOpen ?? new RelayCommand((object o) =>
                {
                    IsEnabledPecentageScreen = Visibility.Collapsed;
                    IsEnabledGreenRedScreen = Visibility.Collapsed;
                    IsEnabledVolatilityTodayScreen = Visibility.Collapsed;
                    IsEnabledVolatilityWeeklyScreen = Visibility.Visible;
                });
            }
        }
        private RelayCommand volatilityWeeklyScreenOpen;
    }
}
