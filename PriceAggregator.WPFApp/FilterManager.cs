using PriceAggregator.WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp
{
    public class FilterManager : PropertyChangedBase
    {
        private ObservableCollection<PercentageView> percentageViews;

        public FilterManager(ObservableCollection<PercentageView> percentageViews)
        {
            this.percentageViews = percentageViews;
        }

        public bool IsFilterApplied { get; set; }

        public bool IsEnabledFilter
        {
            get { return isEnabledFilter; }
            set
            {
                isEnabledFilter = value;
                base.NotifyPropertyChanged();
            }
        }
        private bool isEnabledFilter = true;

        public bool IsEnabledShowAll
        {
            get { return isEnabledShowAll; }
            set
            {
                isEnabledShowAll = value;
                base.NotifyPropertyChanged();
            }
        }
        private bool isEnabledShowAll = false;

        public RelayCommand FilterOnCommand
        {
            get
            {
                return filterOnCommand ?? new RelayCommand((object o) =>
                {
                    IsEnabledFilter = false;
                    IsEnabledShowAll = true;
                });
            }
        }
        private RelayCommand filterOnCommand;

        public RelayCommand ShowAllCommand
        {
            get
            {
                return showAllCommand ?? new RelayCommand((object o) =>
                {
                    IsEnabledFilter = true;
                    IsEnabledShowAll = false;
                });
            }
        }
        private RelayCommand showAllCommand;

        public RelayCommand FilterClearCommand
        {
            get
            {
                return filterClearCommand ?? new RelayCommand((object o) =>
                {
                    foreach (var percentageView in percentageViews)
                    {
                        percentageView.IsSelected = false;
                    }
                });
            }
        }
        private RelayCommand filterClearCommand;
    }
}
