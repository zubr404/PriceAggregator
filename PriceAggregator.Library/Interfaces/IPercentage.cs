using PriceAggregator.Library.Percentage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Interfaces
{
    public interface IPercentage
    {
        PercentageChange GetPercentage();
    }
}
