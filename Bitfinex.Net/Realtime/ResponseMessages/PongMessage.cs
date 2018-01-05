using System;
using Bitfinex.Net.Helpers.Attributes;
using Bitfinex.Net.Helpers.JsonConverters;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.ResponseMessages
{
    [RealtimeMessage("pong")]
    internal class PongMessage : RealtimeMessage
    {
        [JsonProperty("cid")]
        public int RequestId { get; private set; }

        [JsonProperty("ts")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Time { get; private set; }
    }
}