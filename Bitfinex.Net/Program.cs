using System;
using System.Threading;
using Bitfinex.Net.Enums;
using Bitfinex.Net.OrderBooks;
using Bitfinex.Net.Realtime;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace Bitfinex.Net
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var bitfinex = new RealtimeBitfinex();
            //bitfinex.ConnectAsync().Wait();
            var bitfinex = new Bitfinex();
            var rawBook = new RawTradingBook(TradingSymbols.BTCUSD);
            bitfinex.UpdateChannel(rawBook).Wait();
        }

        //private static void ViewFundingBook(BitfinexRealtime bitfinex, FundingSymbols symbol)
        //{
        //    var book = bitfinex.OpenChannelAsync(new FundingBook(symbol), CancellationToken.None).Result;
        //    while (true)
        //    {
        //        Console.Clear();
        //        Console.SetCursorPosition(0, 0);
        //        Console.WriteLine(book.UpdatedDate);
        //        Console.WriteLine("--------------------------------------");
        //        foreach (var ask in book.Asks)
        //        {
        //            Console.WriteLine("ASK: {0} of {1} at {2},{3}", ask.Count, ask.Amount, ask.Rate, ask.Period);
        //        }
        //        Console.WriteLine("--------------------------------------");
        //        foreach (var bid in book.Bids)
        //        {
        //            Console.WriteLine("BID: {0} of {1} at {2},{3}", bid.Count, bid.Amount, bid.Rate, bid.Period);
        //        }
        //        Thread.Sleep(300);
        //    }
        //}
        //private static void ViewTradingBook(BitfinexRealtime bitfinex, TradingSymbols symbol)
        //{
        //    var book = bitfinex.OpenChannelAsync(new TradingBook(symbol), CancellationToken.None).Result;
        //    while (true)
        //    {
        //        Console.Clear();
        //        Console.SetCursorPosition(0, 0);
        //        Console.WriteLine(book.UpdatedDate);
        //        Console.WriteLine("--------------------------------------");
        //        foreach (var ask in book.Asks)
        //        {
        //            Console.WriteLine("ASK: {0} of {1} at {2}", ask.Count, ask.Amount, ask.Price);
        //        }
        //        Console.WriteLine("--------------------------------------");
        //        foreach (var bid in book.Bids)
        //        {
        //            Console.WriteLine("BID: {0} of {1} at {2}", bid.Count, bid.Amount, bid.Price);
        //        }
        //        Thread.Sleep(300);
        //    }
        //}
        
    }
}