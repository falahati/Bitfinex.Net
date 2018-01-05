using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Bitfinex.Net
{
    public class Bitfinex
    {
        protected const string Url = "https://api.bitfinex.com/v2/";

        public Bitfinex()
        {

        }

        public Bitfinex(string apiKey, string apiSecret)
        {
            ApiKey = string.IsNullOrWhiteSpace(apiKey) ? null : apiKey;
            ApiSecret = string.IsNullOrWhiteSpace(apiSecret) ? null : apiSecret;
        }

        protected string ApiKey { get; set; }
        protected string ApiSecret { get; set; }

        public virtual async Task UpdateChannel(IChannel channel)
        {
            var webClient = new WebClient();
            var response = await webClient.DownloadStringTaskAsync(new Uri(Url + "book/tBTCUSD/P0"));
            var r = ChannelResponse.Deserialize(response);
            channel.OnChannelResponse(r);
        }
    }
}