using System;
using Bitfinex.Net.Realtime.ResponseMessages;

namespace Bitfinex.Net.Realtime
{
    public interface IRealtimeChannel : IChannel
    {
        int ChannelId { get; }
        DateTime LastUpdate { get; }

        RealtimeMessage GetSubscriptionMessage();
        RealtimeMessage GetUnsubscriptionMessage();
        void OnChannelSubscribed(SubscribedMessage subscribedMessage);
        void OnChannelUnsubscribed(UnsubscribedMessage unsubscribedMessage);

        event EventHandler<EventArgs> Updated;
    }
}