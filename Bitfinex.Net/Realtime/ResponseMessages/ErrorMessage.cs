using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.ResponseMessages
{
    [RealtimeMessage("error")]
    internal class ErrorMessage : RealtimeMessage
    {
        [JsonProperty("code")]
        public int Code { get; private set; }

        [JsonProperty("msg")]
        public string Message { get; private set; }
    }
}