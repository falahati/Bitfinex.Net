using System;
using System.Collections.Generic;
using System.Linq;
using Bitfinex.Net.Enums;

namespace Bitfinex.Net.OrderBooks
{
    public class RawTradingBook : IOrderBook<RawTradingBookRecord>
    {
        protected readonly List<RawTradingBookRecord> Orders = new List<RawTradingBookRecord>();

        public RawTradingBook(TradingSymbols symbol, BooksLimit limit = BooksLimit.TwentyFive)
        {
            Symbol = symbol;
            Length = limit;
        }

        public BooksLimit Length { get; protected set; }
        public TradingSymbols Symbol { get; protected set; }

        /// <inheritdoc />
        public void OnChannelResponse(ChannelResponse response)
        {
            lock (this)
            {
                if (response.DataList != null)
                    foreach (var data in response.DataList)
                        WriteRecord(data);
            }
        }


        public RawTradingBookRecord[] Asks
        {
            get
            {
                lock (Orders)
                {
                    return
                        Orders.Where(record => record.RecordType == BookRecordType.Ask)
                            .OrderByDescending(book => book.Price)
                            .ToArray();
                }
            }
        }

        public RawTradingBookRecord[] Bids
        {
            get
            {
                lock (Orders)
                {
                    return
                        Orders.Where(record => record.RecordType == BookRecordType.Bid)
                            .OrderByDescending(book => book.Price)
                            .ToArray();
                }
            }
        }

        protected void WriteRecord(string record)
        {
            var newOrder = RawTradingBookRecord.Deserialize(record);
            if (newOrder == null)
                return;
            lock (Orders)
            {
                var oldOrder = Orders.FirstOrDefault(o => o.Equals(newOrder));
                if (oldOrder == null)
                    Orders.Add(newOrder);
                else if (Math.Abs(newOrder.Price) < double.Epsilon)
                    Orders.Remove(oldOrder);
                else
                    oldOrder.Update(newOrder);
            }
        }
    }
}