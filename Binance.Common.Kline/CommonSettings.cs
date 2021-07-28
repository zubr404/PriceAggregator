using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Common.Kline
{
    /// <summary>
    /// Общие для всего проекта настройки
    /// </summary>
    public class CommonSettings
    {
        public const int LIMIT_KLINES = 100;
        public static readonly TimeSpan RESTART_STREAM_TIME = new TimeSpan(2, 27, 00);
        public static readonly TimeSpan INTERVAL_CHANNEL_RESTART = new TimeSpan(0, 0, 5);
        public const int COUNT_CANDLE_DEPTH = 6;

        #region SelectedSimbols
        private readonly List<string> selectedSimbols = new List<string>();

        public IReadOnlyCollection<string> GetSelectedSimbols()
        {
            return new ReadOnlyCollection<string>(selectedSimbols);
        }

        public void SetSelectedSimbols(IEnumerable<string> simbols)
        {
            if (simbols?.Count() > 0)
            {
                selectedSimbols.Clear();
                foreach (var simbol in simbols)
                {
                    selectedSimbols.Add(simbol);
                }
            }
        }
        #endregion

        #region SelectedInterval
        private string selectedInterval;
        public string SelectedInterval
        {
            get { return selectedInterval; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    selectedInterval = value;
                }
            }
        }
        #endregion

        #region SelectedPercentageAlert
        public decimal SelectedPercentageAlert { get; set; }
        #endregion

        #region BotToken
        public string BotToken { get; set; }
        #endregion

        #region ChatId
        public string ChatId { get; set; }
        #endregion
    }
}
