using Bitfinex.Net.Helpers.Attributes;

namespace Bitfinex.Net.Enums
{
    public enum CandlesTimeFrame
    {
        [EnumStringValue("1m")] OneMinute,
        [EnumStringValue("5m")] FiveMinutes,
        [EnumStringValue("15m")] FifteenMinutes,
        [EnumStringValue("30m")] ThirtyMinutes,
        [EnumStringValue("1h")] OneHour,
        [EnumStringValue("3h")] ThreeHours,
        [EnumStringValue("6h")] SixHours,
        [EnumStringValue("12h")] TwelveHours,
        [EnumStringValue("1D")] OneDay,
        [EnumStringValue("7D")] SevenDays,
        [EnumStringValue("14D")] ForteenDays,
        [EnumStringValue("1M")] OneMonth
    }
}