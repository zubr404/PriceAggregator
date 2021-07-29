using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Percentage
{
    public class PercentageChangeComparer : IEqualityComparer<PercentageChange>
    {
        public bool Equals(PercentageChange x, PercentageChange y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Simbol == y.Simbol && x.Interval == y.Interval && x.Percentage == y.Percentage;
        }

        public int GetHashCode(PercentageChange percentageChange)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(percentageChange, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductSimbol = percentageChange.Simbol == null ? 0 : percentageChange.Simbol.GetHashCode();
            int hashProductInterval = percentageChange.Interval == null ? 0 : percentageChange.Interval.GetHashCode();
            int hashProductPercentage = percentageChange.Percentage == null ? 0 : percentageChange.Percentage.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductSimbol ^ hashProductInterval ^ hashProductPercentage;
        }
    }
}
