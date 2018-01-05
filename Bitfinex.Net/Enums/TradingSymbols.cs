using Bitfinex.Net.Helpers.Attributes;

namespace Bitfinex.Net.Enums
{
    public enum TradingSymbols
    {
        [EnumStringValue("tBTCUSD")]
        BTCUSD,
        [EnumStringValue("tLTCUSD")]
        LTCUSD,
        [EnumStringValue("tLTCBTC")]
        LTCBTC,
        [EnumStringValue("tETHUSD")]
        ETHUSD,
        [EnumStringValue("tETHBTC")]
        ETHBTC
    }
}