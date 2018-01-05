using System;
using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.RequestMessages
{
    [RealtimeMessage("ping")]
    public class PingMessage : RealtimeMessage
    {
        public PingMessage() : this(new Random().Next(0, int.MaxValue))
        {
        }

        public PingMessage(int requestId)
        {
            RequestId = requestId;
        }

        [JsonProperty("cid")]
        public int RequestId { get; private set; }
    }
}