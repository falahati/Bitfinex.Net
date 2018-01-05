using System.Linq;
using Newtonsoft.Json;

namespace Bitfinex.Net
{
    public class ChannelResponse
    {
        internal ChannelResponse(string[] dataList)
        {
            DataList = dataList;
        }

        public string[] DataList { get; protected set; }

        public static ChannelResponse Deserialize(string serialized)
        {
            if (!serialized.StartsWith("["))
                return null;
            var oneLevel = JsonConvert.DeserializeObject<object[]>(serialized);
            return oneLevel.All(o => !o.GetType().IsClass)
                ? new ChannelResponse(new[] {serialized})
                : new ChannelResponse(oneLevel.Select(o => o.ToString()).ToArray());
        }
    }
}