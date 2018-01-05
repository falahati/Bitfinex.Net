using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.ResponseMessages
{
    [RealtimeMessage("unsubscribed")]
    public class UnsubscribedMessage : RealtimeMessage
    {
        [JsonProperty("chanId")]
        public int ChannelId { get; private set; }

        [JsonProperty("channel")]
        public string ChannelName { get; private set; }

        [JsonProperty("freq")]
        public string Frequency { get; private set; }

        [JsonProperty("key")]
        public string Key { get; private set; }

        [JsonProperty("len")]
        public string Length { get; private set; }

        [JsonProperty("pair")]
        public string Pair { get; private set; }

        [JsonProperty("prec")]
        public string Precision { get; private set; }

        [JsonProperty("symbol")]
        public string Symbol { get; private set; }
    }
}