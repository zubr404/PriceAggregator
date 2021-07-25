using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.WPFApp.ViewModels
{
    public class SimbolView : PropertyChangedBase
    {
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
    }
}
