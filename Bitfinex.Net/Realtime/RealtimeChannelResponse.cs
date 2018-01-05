using System;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime
{
    public class RealtimeChannelResponse : ChannelResponse
    {
        internal RealtimeChannelResponse(int channelId, string[] dataList) : base(dataList)
        {
            ChannelId = channelId;
        }

        public int ChannelId { get; private set; }

        public new static RealtimeChannelResponse Deserialize(string serialized)
        {
            if (!serialized.StartsWith("["))
                return null;
            var response = JsonConvert.DeserializeObject<object[]>(serialized);
            int channelId;
            if (!int.TryParse(response[0].ToString(), out channelId))
                return null;
            if (response[1].ToString().Equals("hb", StringComparison.InvariantCultureIgnoreCase))
                return new RealtimeChannelResponse(channelId, null);
            var channelResponse = ChannelResponse.Deserialize(response[1].ToString());
            return channelResponse != null ? new RealtimeChannelResponse(channelId, channelResponse.DataList) : null;
        }
    }
}