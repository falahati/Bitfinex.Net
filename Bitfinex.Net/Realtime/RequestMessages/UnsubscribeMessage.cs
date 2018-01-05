using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.RequestMessages
{
    [RealtimeMessage("unsubscribe")]
    public class UnsubscribeMessage : RealtimeMessage
    {
        public UnsubscribeMessage(int channelId)
        {
            ChannelId = channelId;
        }

        [JsonProperty("chanId")]
        public int ChannelId { get; private set; }
    }
}