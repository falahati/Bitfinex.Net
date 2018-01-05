using System;
using Newtonsoft.Json;

namespace Bitfinex.Net.Helpers.JsonConverters
{
    public class TimestampConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var ts = long.Parse(reader.Value.ToString());
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ts).ToLocalTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var datetime = value as DateTime?;
            if (datetime.HasValue)
            {
                writer.WriteValue(datetime.Value.ToUniversalTime().Ticks);
                return;
            }
            writer.WriteValue(0L);
        }
    }
}