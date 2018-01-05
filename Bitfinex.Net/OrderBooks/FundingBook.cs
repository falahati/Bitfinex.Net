//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Bitfinex.Net.Enums;
//using Bitfinex.Net.Requests;
//using Bitfinex.Net.Responses;

//namespace Bitfinex.Net.OrderBooks
//{
//    public class FundingBook : IChannel
//    {
//        private readonly List<FundingBookRecord> _asks = new List<FundingBookRecord>();
//        private readonly List<FundingBookRecord> _bids = new List<FundingBookRecord>();

//        public FundingBook(FundingSymbols symbol, BooksPrecision precision = BooksPrecision.NoAggregation,
//            BooksFrequency frequency = BooksFrequency.Realtime, BooksLimit limit = BooksLimit.TwentyFive)
//        {
//            Symbol = symbol;
//            Precision = precision;
//            Frequency = frequency;
//            Length = limit;
//        }


//        public FundingBookRecord[] Asks
//        {
//            get
//            {
//                lock (_asks)
//                {
//                    return _asks.OrderByDescending(book => book.Rate).ToArray();
//                }
//            }
//        }

//        public FundingBookRecord[] Bids
//        {
//            get
//            {
//                lock (_bids)
//                {
//                    return _bids.OrderByDescending(book => book.Rate).ToArray();
//                }
//            }
//        }

//        public BooksFrequency Frequency { get; }

//        public BooksLimit Length { get; }
//        public BooksPrecision Precision { get; }
//        public FundingSymbols Symbol { get; }
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
//            if (data.Length == 4)
//            {
//                var rate = double.Parse(data[0].ToString());
//                var period = double.Parse(data[1].ToString());
//                var count = uint.Parse(data[2].ToString());
//                var amount = double.Parse(data[3].ToString());

//                if (count == 0)
//                {
//                    if (amount < 0)
//                        lock (_bids)
//                        {
//                            var bid =
//                                _bids.FirstOrDefault(
//                                    book =>
//                                        (Math.Abs(book.Rate - rate) < double.Epsilon) &&
//                                        (Math.Abs(book.Period - period) < double.Epsilon));
//                            if (bid != null)
//                                _bids.Remove(bid);
//                        }
//                    else if (amount > 0)
//                        lock (_asks)
//                        {
//                            var ask =
//                                _asks.FirstOrDefault(
//                                    book =>
//                                        (Math.Abs(book.Rate - rate) < double.Epsilon) &&
//                                        (Math.Abs(book.Period - period) < double.Epsilon));
//                            if (ask != null)
//                                _asks.Remove(ask);
//                        }
//                }
//                else
//                {
//                    if (amount < 0)
//                        lock (_bids)
//                        {
//                            var bid =
//                                _bids.FirstOrDefault(
//                                    book =>
//                                        (Math.Abs(book.Rate - rate) < double.Epsilon) &&
//                                        (Math.Abs(book.Period - period) < double.Epsilon));
//                            if (bid != null)
//                                bid.Update(amount, count);
//                            else
//                                _bids.Add(new FundingBookRecord(rate, period, amount, count));
//                        }
//                    else if (amount > 0)
//                        lock (_asks)
//                        {
//                            var ask =
//                                _asks.FirstOrDefault(
//                                    book =>
//                                        (Math.Abs(book.Rate - rate) < double.Epsilon) &&
//                                        (Math.Abs(book.Period - period) < double.Epsilon));
//                            if (ask != null)
//                                ask.Update(amount, count);
//                            else
//                                _asks.Add(new FundingBookRecord(rate, period, amount, count));
//                        }
//                }
//            }
//        }
//    }
//}