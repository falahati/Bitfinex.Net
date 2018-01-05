using Bitfinex.Net.Helpers.Attributes;

namespace Bitfinex.Net.Enums
{
    public enum BooksPrecision
    {
        [EnumStringValue("P0")] NoAggregation,
        [EnumStringValue("P1")] AggregateToTen,
        [EnumStringValue("P2")] AggregateToHundred,
        [EnumStringValue("P3")] AggregateToThousand
    }
}