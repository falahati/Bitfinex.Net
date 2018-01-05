//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Bitfinex.Net.Enums;
//using Bitfinex.Net.Requests;
//using Bitfinex.Net.Responses;

//namespace Bitfinex.Net.OrderBooks
//{
//    public class TradingBook : IRealtimeChannel
//    {
//        private readonly List<TradingBookRecord> _asks = new List<TradingBookRecord>();
//        private readonly List<TradingBookRecord> _bids = new List<TradingBookRecord>();

//        public TradingBook(TradingSymbols symbol, BooksPrecision precision = BooksPrecision.NoAggregation,
//            BooksFrequency frequency = BooksFrequency.Realtime, BooksLimit limit = BooksLimit.TwentyFive)
//        {
//            Symbol = symbol;
//            Precision = precision;
//            Frequency = frequency;
//            Length = limit;
//        }


//        public TradingBookRecord[] Asks
//        {
//            get
//            {
//                lock (_asks)
//                {
//                    return _asks.OrderByDescending(book => book.Price).ToArray();
//                }
//            }
//        }

//        public TradingBookRecord[] Bids
//        {
//            get
//            {
//                lock (_bids)
//                {
//                    return _bids.OrderByDescending(book => book.Price).ToArray();
//                }
//            }
//        }

//        public BooksFrequency Frequency { get; }

//        public BooksLimit Length { get; }
//        public BooksPrecision Precision { get; }
//        public TradingSymbols Symbol { get; }
//        public DateTime UpdatedDate { get; set; }

//        /// <inheritdoc />
//        public int ChannelId { get; private set; }

//        /// <inheritdoc />
//        public Message GetSubscriptionMessage()
//        {
//            return SubscribeMessage.ToOrderBook(Symbol, Precision, Frequency, Length);
//        }

//        /// <inheritdoc />
//        public Message GetUnsubscriptionMessage()
//        {
//            throw new NotImplementedException();
//        }

//        /// <inheritdoc />
//        public void OnChannelSubscribed(SubscribedMessage channel)
//        {
//            ChannelId = channel.ChannelId;
//        }

//        /// <inheritdoc />
//        public void OnChannelUnsubscribed(int channelId)
//        {
//            ChannelId = 0;
//        }

//        /// <inheritdoc />
//        public void OnChannelResponse(ChannelUpdate update)
//        {
//            lock (this)
//            {
//                UpdatedDate = DateTime.Now;
//                if (update.DataList != null)
//                    foreach (var data in update.DataList)
//                        WriteRecord(data);
//            }
//            Updated?.Invoke(this, new EventArgs());
//        }

//        public event EventHandler<EventArgs> Updated;

//        protected void WriteRecord(object[] data)
//        {
//            if (data.Length == 3)
//            {
//                var price = double.Parse(data[0].ToString());
//                var count = uint.Parse(data[1].ToString());
//                var amount = double.Parse(data[2].ToString());
//                if (count == 0)
//                {
//                    if (amount > 0)
//                        lock (_bids)
//                        {
//                            var bid = _bids.FirstOrDefault(book => Math.Abs(book.Price - price) < double.Epsilon);
//                            if (bid != null)
//                                _bids.Remove(bid);
//                        }
//                    else if (amount < 0)
//                        lock (_asks)
//                        {
//                            var ask = _asks.FirstOrDefault(book => Math.Abs(book.Price - price) < double.Epsilon);
//                            if (ask != null)
//                                _asks.Remove(ask);
//                        }
//                }
//                else
//                {
//                    if (amount > 0)
//                        lock (_bids)
//                        {
//                            var bid = _bids.FirstOrDefault(book => Math.Abs(book.Price - price) < double.Epsilon);
//                            if (bid != null)
//                                bid.Update(amount, count);
//                            else
//                                _bids.Add(new TradingBookRecord(price, amount, count));
//                        }
//                    else if (amount < 0)
//                        lock (_asks)
//                        {
//                            var ask = _asks.FirstOrDefault(book => Math.Abs(book.Price - price) < double.Epsilon);
//                            if (ask != null)
//                                ask.Update(amount, count);
//                            else
//                                _asks.Add(new TradingBookRecord(price, amount, count));
//                        }
//                }
//            }
//        }
//    }
//}