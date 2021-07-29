using Binance.Common.Kline;
using PriceAggregator.Library;
using PriceAggregator.Library.Percentage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Telegram.Bot;

namespace Alerts
{
    public class TelegramAlert
    {
        private readonly CommonSettings commonSettings;
        private readonly PriceAggregatorManager priceAggregatorManager;

        private readonly Timer timer;

        public TelegramAlert(CommonSettings commonSettings, PriceAggregatorManager priceAggregatorManager)
        {
            this.commonSettings = commonSettings;
            this.priceAggregatorManager = priceAggregatorManager;

            timer = new Timer(2000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            percentagesForIntervalPrevios = new PercentageChange[] { };
        }

        private IEnumerable<PercentageChange> percentagesForIntervalPrevios;
        private string selectedIntervalPrevios = "";
        private decimal selectedPercentageAlertPrevios = 0;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(commonSettings.BotToken) || string.IsNullOrWhiteSpace(commonSettings.ChatId)) { return; }

            if (priceAggregatorManager.PercentageChanges?.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(commonSettings.SelectedInterval) && commonSettings.SelectedPercentageAlert != 0)
                {
                    if (commonSettings.SelectedInterval != selectedIntervalPrevios || commonSettings.SelectedPercentageAlert != selectedPercentageAlertPrevios)
                    {
                        selectedIntervalPrevios = commonSettings.SelectedInterval;
                        selectedPercentageAlertPrevios = commonSettings.SelectedPercentageAlert;
                        percentagesForIntervalPrevios = new PercentageChange[] { };
                    }

                    if (commonSettings.SelectedPercentageAlert > 0)
                    {
                        var percentagesForInterval = priceAggregatorManager.PercentageChanges.Where(x => x.Interval == commonSettings.SelectedInterval && x.Percentage > commonSettings.SelectedPercentageAlert);
                        alertRun(percentagesForInterval, commonSettings.BotToken, commonSettings.ChatId);
                    }
                    else if (commonSettings.SelectedPercentageAlert < 0)
                    {
                        var percentagesForInterval = priceAggregatorManager.PercentageChanges.Where(x => x.Interval == commonSettings.SelectedInterval && x.Percentage < commonSettings.SelectedPercentageAlert);
                        alertRun(percentagesForInterval, commonSettings.BotToken, commonSettings.ChatId);
                    }
                }
            }
        }

        private void alertRun(IEnumerable<PercentageChange> percentagesForInterval, string tokenBot, string chatId)
        {
            var alertPercentages = percentagesForInterval.Except(percentagesForIntervalPrevios, new PercentageChangeComparer());

            if (alertPercentages?.Count() > 0)
            {
                var stringBuilders = new List<StringBuilder>();
                for (int i = 0; i < alertPercentages.Count(); i += 100)
                {
                    var pageAlertPercentages = alertPercentages.Skip(i).Take(100);
                    var sb = new StringBuilder();
                    foreach (var alertPercentage in pageAlertPercentages)
                    {
                        sb.Append($"{alertPercentage.Simbol} ");
                        sb.Append($"{alertPercentage.Interval} ");
                        sb.Append($"{Math.Round(alertPercentage.Percentage.Value, 2)}\n");
                    }
                    stringBuilders.Add(sb);
                }

                foreach (var stringBuilder in stringBuilders)
                {
                    try
                    {
                        var bot = new TelegramBotClient(tokenBot);
                        var outMessage = bot.SendTextMessageAsync(chatId, stringBuilder.ToString()).GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            percentagesForIntervalPrevios = percentagesForInterval;
        }
    }
}
