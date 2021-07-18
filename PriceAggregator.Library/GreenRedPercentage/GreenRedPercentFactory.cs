using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.GreenRedPercentage
{
    class GreenRedPercentFactory
    {
        private readonly IRepository repository;
        private readonly IEnumerable<string> simbols;
        private readonly IEnumerable<string> intervals;

        public GreenRedPercentFactory(IRepository repository, IEnumerable<string> simbols, IEnumerable<string> intervals)
        {
            this.repository = repository;
            this.simbols = simbols;
            this.intervals = intervals;
        }

        public IList<IGreenRedPercent> GetPercentageModls()
        {
            var result = new List<IGreenRedPercent>();
            if (simbols?.Count() > 0)
            {
                if (intervals?.Count() > 0)
                {
                    foreach (var simbol in simbols)
                    {
                        foreach (var interval in intervals)
                        {
                            if (KlineTimeframe.TimeframesAdaptive.Contains(interval)) // если интервал совпадает с Бинанс
                            {
                                result.Add(new GreenRedPercentMatchInterval(repository, simbol, interval));
                            }
                            else
                            {
                                switch (interval)
                                {
                                    case KlineTimeframe.hour3:
                                        result.Add(new GreenRedPercentHour3(repository, simbol, interval));
                                        break;
                                    case KlineTimeframe.day2:
                                        result.Add(new GreenRedPercentDay2(repository, simbol, interval));
                                        break;
                                    case KlineTimeframe.day5:
                                        result.Add(new GreenRedPercentDay5(repository, simbol, interval));
                                        break;
                                    case KlineTimeframe.weekly2:
                                        result.Add(new GreenRedPercentWeekly2(repository, simbol, interval));
                                        break;
                                    case KlineTimeframe.month2:
                                        result.Add(new GreenRedPercentMonth2(repository, simbol, interval));
                                        break;
                                    case KlineTimeframe.month3:
                                        result.Add(new GreenRedPercentMonth3(repository, simbol, interval));
                                        break;
                                    case KlineTimeframe.month6:
                                        result.Add(new GreenRedPercentMonth6(repository, simbol, interval));
                                        break;
                                    case KlineTimeframe.year1:
                                        result.Add(new GreenRedPercentYear1(repository, simbol, interval));
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
