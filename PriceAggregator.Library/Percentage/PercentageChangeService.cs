using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Percentage
{
    public class PercentageChangeService
    {
        private readonly IRepository repository;

        public PercentageChangeService(IRepository repository)
        {
            this.repository = repository;
        }

        public List<PercentageChange> GetPercentages(IEnumerable<string> simbols, IEnumerable<string> intervals)
        {
            var result = new List<PercentageChange>();
            var factory = new PercentageFactory(repository, simbols, intervals);
            var percentageModels = factory.GetPercentageModls();

            foreach (var percentageModel in percentageModels)
            {
                result.Add(percentageModel.GetPercentage());
            }

            return result;
        }
    }
}
