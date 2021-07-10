using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebSocketSharp;

namespace Binance.StockExchange.Kline.WS
{
    public class KlineStreamService
    {
        public const string SOCKET = "wss://stream.binance.com:9443/ws/";
        private WebSocket webSocket;
        private readonly string channels;
        private readonly IRepository repository;
        private readonly Timer timer;
        private bool isRestartStream;

        private DateTime restartDateTime;

        public KlineStreamService(IRepository repository, string channels, TimeSpan restartSocketTime)
        {
            this.channels = channels;
            this.repository = repository;
            this.restartDateTime = DateTime.Now.Date + restartSocketTime;
            isRestartStream = true;
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now > restartDateTime)
            {
                restartDateTime = restartDateTime.AddDays(1);
                socketClose();
            }
        }

        public void SocketOpen()
        {
            try
            {
                socketOpen();
                isRestartStream = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void socketOpen()
        {
            webSocket = new WebSocket($"{SOCKET}{channels}");
            webSocket.OnMessage += WebSocket_OnMessage;
            webSocket.OnError += WebSocket_OnError;
            webSocket.OnClose += WebSocket_OnClose;
            webSocket.OnOpen += WebSocket_OnOpen;
            webSocket.Connect();
        }

        public void SocketClose()
        {
            isRestartStream = false;
            socketClose();
        }
        private void socketClose()
        {
            webSocket?.Close();
        }

        private void restartSocket()
        {
            if (isRestartStream)
            {
                SocketOpen();
            }
        }

        private void WebSocket_OnOpen(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} Connect open {restartDateTime}");
            Console.ResetColor();
        }

        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now} Connect close {restartDateTime}");
            Console.ResetColor();

            restartSocket();
        }

        private void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} Error: {e.Message}");
            Console.ResetColor();

            socketClose();
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
                //Console.WriteLine($"{candle.TimeOpen} {candle.Simbol} {candle.Close} {candle.IsClose}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
