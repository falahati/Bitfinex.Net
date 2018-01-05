using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.ResponseMessages
{
    [RealtimeMessage("info")]
    internal class InfoMessage : RealtimeMessage
    {
        public enum InfoCode : uint
        {
            None = 0,
            PleaseReconnect = 20051,
            MaintenanceStarted = 20060,
            MaintenanceEnded = 20061
        }

        [JsonProperty("code")]
        public InfoCode Code { get; private set; }

        [JsonProperty("msg")]
        public string Message { get; private set; }

        [JsonProperty("version")]
        public uint Version { get; private set; }
    }
}