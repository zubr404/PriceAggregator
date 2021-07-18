using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.GreenRedPercentage
{
    public class GreenRedPercentService
    {
        private readonly IRepository repository;

        public GreenRedPercentService(IRepository repository)
        {
            this.repository = repository;
        }

        public List<GreenRedPercentChange> GetPercentages(IEnumerable<string> simbols, IEnumerable<string> intervals)
        {
            var result = new List<GreenRedPercentChange>();
            var factory = new GreenRedPercentFactory(repository, simbols, intervals);
            var percentageModels = factory.GetPercentageModls();

            foreach (var percentageModel in percentageModels)
            {
                result.Add(percentageModel.GetPercentage());
            }

            return result;
        }
    }
}
