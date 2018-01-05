using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.RequestMessages
{
    [RealtimeMessage("conf")]
    public class ConfigureMessage : RealtimeMessage
    {
        [JsonIgnore]
        public bool Checksum
        {
            get { return (Flags & 0x00020000) == 0x00020000; }
            set
            {
                if (value)
                    Flags |= 0x00020000;
                else
                    Flags &= ~0x00020000;
            }
        }

        [JsonIgnore]
        public bool DecimalAsString
        {
            get { return (Flags & 0x00000008) == 0x00000008; }
            set
            {
                if (value)
                    Flags |= 0x00000008;
                else
                    Flags &= ~0x00000008;
            }
        }

        [JsonProperty("flags")]
        public int Flags { get; private set; }

        [JsonIgnore]
        public bool SequencingBeta
        {
            get { return (Flags & 0x00010000) == 0x00010000; }
            set
            {
                if (value)
                    Flags |= 0x00010000;
                else
                    Flags &= ~0x00010000;
            }
        }

        [JsonIgnore]
        public bool TimeAsString
        {
            get { return (Flags & 0x00000020) == 0x00000020; }
            set
            {
                if (value)
                    Flags |= 0x00000020;
                else
                    Flags &= ~0x00000020;
            }
        }
    }
}