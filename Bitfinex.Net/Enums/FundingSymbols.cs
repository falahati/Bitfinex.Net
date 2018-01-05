using Bitfinex.Net.Helpers.Attributes;

namespace Bitfinex.Net.Enums
{
    public enum FundingSymbols
    {
        [EnumStringValue("fUSD")]
        UnitedStatesDollar,
        [EnumStringValue("fBTC")]
        Bitcoin
    }
}