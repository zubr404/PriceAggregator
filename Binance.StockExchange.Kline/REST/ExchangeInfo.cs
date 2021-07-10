using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.StockExchange.Kline.REST
{
    public class ExchangeInfo
    {
        public string Info { get; private set; }
        public AllPairsMarket AllPairsMarket { get; private set; }

        private readonly PublicRequester publicRequester;
        public ExchangeInfo()
        {
            publicRequester = new PublicRequester();
            AllPairsMarket = new AllPairsMarket(GetInfo());
        }
        public string GetInfo()
        {
            try
            {
                var response = publicRequester.RequestPublicApi($"https://api.binance.com/api/v1/exchangeInfo");
                Info = response.Result.ResponseMessage;
                return Info;
            }
            catch (Exception)
            {
                // TODO: Сохранеие логово
                throw;
            }
        }
    }
}
