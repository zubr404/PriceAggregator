using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.ViewModels
{
    public class PercentageView : PropertyChangedBase
    {
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                base.NotifyPropertyChanged();
            }
        }
        private bool isSelected;
        public string Simbol
        {
            get { return simbol; }
            set
            {
                simbol = value;
                base.NotifyPropertyChanged();
            }
        }
        private string simbol;
        public decimal? Percentage1m
        {
            get { return percentage1m; }
            set
            {
                percentage1m = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage1m;
        public decimal? Percentage5m
        {
            get { return percentage5m; }
            set
            {
                percentage5m = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage5m;
        public decimal? Percentage15m
        {
            get { return percentage15m; }
            set
            {
                percentage15m = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage15m;
        public decimal? Percentage30m
        {
            get { return percentage30m; }
            set
            {
                percentage30m = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage30m;
        public decimal? Percentage1h
        {
            get { return percentage1h; }
            set
            {
                percentage1h = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage1h;
        public decimal? Percentage3h
        {
            get { return percentage3h; }
            set
            {
                percentage3h = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage3h;
        public decimal? Percentage6h
        {
            get { return percentage6h; }
            set
            {
                percentage6h = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage6h;
        public decimal? Percentage12h
        {
            get { return percentage12h; }
            set
            {
                percentage12h = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage12h;
        public decimal? Percentage1d
        {
            get { return percentage1d; }
            set
            {
                percentage1d = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage1d;
        public decimal? Percentage2d
        {
            get { return percentage2d; }
            set
            {
                percentage2d = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage2d;
        public decimal? Percentage3d
        {
            get { return percentage3d; }
            set
            {
                percentage3d = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage3d;
        public decimal? Percentage5d
        {
            get { return percentage5d; }
            set
            {
                percentage5d = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage5d;
        public decimal? Percentage1w
        {
            get { return percentage1w; }
            set
            {
                percentage1w = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage1w;
        public decimal? Percentage2w
        {
            get { return percentage2w; }
            set
            {
                percentage2w = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage2w;
        public decimal? Percentage1M
        {
            get { return percentage1M; }
            set
            {
                percentage1M = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage1M;
        public decimal? Percentage2M
        {
            get { return percentage2M; }
            set
            {
                percentage2M = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage2M;
        public decimal? Percentage3M
        {
            get { return percentage3M; }
            set
            {
                percentage3M = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage3M;
        public decimal? Percentage6M
        {
            get { return percentage6M; }
            set
            {
                percentage6M = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage6M;
        public decimal? Percentage1Y
        {
            get { return percentage1Y; }
            set
            {
                percentage1Y = value;
                base.NotifyPropertyChanged();
            }
        }
        private decimal? percentage1Y;
    }
}
