using Bitfinex.Net.Helpers.Attributes;

namespace Bitfinex.Net.Enums
{
    public enum BooksFrequency
    {
        [EnumStringValue("F0")]
        Realtime,
        [EnumStringValue("F1")]
        TwoSeconds
    }
}