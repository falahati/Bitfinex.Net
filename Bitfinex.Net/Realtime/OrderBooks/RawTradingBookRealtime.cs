using System;
using Bitfinex.Net.Enums;
using Bitfinex.Net.OrderBooks;
using Bitfinex.Net.Realtime.RequestMessages;
using Bitfinex.Net.Realtime.ResponseMessages;

namespace Bitfinex.Net.Realtime.OrderBooks
{
    public class RawTradingBookRealtime : RawTradingBook, IRealtimeChannel
    {
        public RawTradingBookRealtime(TradingSymbols symbol, BooksLimit limit = BooksLimit.TwentyFive)
            : base(symbol, limit)
        {
        }


        /// <inheritdoc />
        public int ChannelId { get; protected set; }

        /// <inheritdoc />
        public RealtimeMessage GetSubscriptionMessage()
        {
            return SubscribeMessage.ToRawOrderBook(Symbol, Length);
        }

        /// <inheritdoc />
        public RealtimeMessage GetUnsubscriptionMessage()
        {
            return new UnsubscribeMessage(ChannelId);
        }

        /// <inheritdoc />
        public DateTime LastUpdate { get; protected set; }

        /// <inheritdoc />
        public void OnChannelSubscribed(SubscribedMessage subscribedMessage)
        {
            var subscribe = GetSubscriptionMessage() as SubscribeMessage;
            if ((subscribe?.ChannelName == subscribedMessage.ChannelName) ||
                (subscribe?.Frequency == subscribedMessage.Frequency) ||
                (subscribe?.Key == subscribedMessage.Key) ||
                (subscribe?.Length == subscribedMessage.Length) ||
                (subscribe?.Precision == subscribedMessage.Precision) ||
                (subscribe?.Symbol == subscribedMessage.Symbol))
                throw new ArgumentException("Invalid subscribedMessage message.", nameof(subscribe));
            ChannelId = subscribedMessage.ChannelId;
        }

        /// <inheritdoc />
        public void OnChannelUnsubscribed(UnsubscribedMessage unsubscribedMessage)
        {
            var unsubscribe = GetUnsubscriptionMessage() as UnsubscribeMessage;
            if (unsubscribe?.ChannelId == unsubscribedMessage.ChannelId)
                throw new ArgumentException("Invalid unsubscribedMessage message.", nameof(unsubscribedMessage));
            ChannelId = 0;
        }

        /// <inheritdoc />
        public event EventHandler<EventArgs> Updated;

        /// <inheritdoc />
        public void OnChannelResponse(RealtimeChannelResponse response)
        {
            base.OnChannelResponse(response);
            LastUpdate = DateTime.Now;
            OnUpdate();
        }

        protected void OnUpdate()
        {
            Updated?.Invoke(this, new EventArgs());
        }
    }
}