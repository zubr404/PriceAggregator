using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Binance.StockExchange.Kline.WS
{
    public class KlineStreamService
    {
        public const string SOCKET = "wss://stream.binance.com:9443/ws/";
        private WebSocket webSocket;
        private readonly string channels;
        private readonly IRepository repository;

        public KlineStreamService(IRepository repository, string channels)
        {
            this.channels = channels;
            this.repository = repository;
        }

        public void SocketOpen()
        {
            try
            {
                webSocket = new WebSocket($"{SOCKET}{channels}");
                webSocket.OnMessage += WebSocket_OnMessage;
                webSocket.OnError += WebSocket_OnError;
                webSocket.OnClose += WebSocket_OnClose;
                webSocket.OnOpen += WebSocket_OnOpen;
                webSocket.Connect();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SocketClose()
        {
            webSocket?.Close();
        }

        private void WebSocket_OnOpen(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} Connect open");
            Console.ResetColor();
        }

        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now} Connect close");
            Console.ResetColor();
        }

        private void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} Error: {e.Message}");
            Console.ResetColor();
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                dynamic kline = JConverter.JsonConver<object>(e.Data);
                var candle = new Candle()
                {
                    TimeOpen = kline.k.t,
                    TimeClose = kline.k.T,
                    Simbol = kline.k.s,
                    Interval = kline.k.i,
                    Open = kline.k.o,
                    Close = kline.k.c,
                    High = kline.k.h,
                    Low = kline.k.l,
                    IsClose = kline.k.x
                };
                repository.CreateOrUpdate(candle);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
