using System;
using System.Linq;
using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime
{
    public class RealtimeMessage
    {
        private static readonly Type[] MessageTypes;

        static RealtimeMessage()
        {
            MessageTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => typeof(RealtimeMessage).IsAssignableFrom(p) && !p.IsAbstract && p.IsClass)
                    .ToArray();
        }

        public RealtimeMessage()
        {
            Event = RealtimeMessageAttribute.GetValue(GetType());
        }

        [JsonProperty("event")]
        public string Event { get; private set; }

        public static RealtimeMessage Deserialize(string serialized)
        {
            if (!serialized.StartsWith("{"))
                return null;
            var response = JsonConvert.DeserializeObject<RealtimeMessage>(serialized);
            if (response?.Event == null)
                return null;
            var responseType =
                MessageTypes.FirstOrDefault(
                    type =>
                        RealtimeMessageAttribute.GetValue(type)
                            .Equals(response.Event, StringComparison.InvariantCultureIgnoreCase));
            if (responseType == null)
                return null;
            return JsonConvert.DeserializeObject(serialized, responseType) as RealtimeMessage;
        }

        public virtual string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}