using Bitfinex.Net.Enums;
using Bitfinex.Net.Helpers.Attributes;
using Newtonsoft.Json;

namespace Bitfinex.Net.Realtime.RequestMessages
{
    [RealtimeMessage("subscribe")]
    public class SubscribeMessage : RealtimeMessage
    {
        private SubscribeMessage()
        {
        }

        [JsonProperty("channel")]
        public string ChannelName { get; private set; }

        [JsonProperty("freq")]
        public string Frequency { get; private set; }

        [JsonProperty("key")]
        public string Key { get; private set; }

        [JsonProperty("len")]
        public string Length { get; private set; }

        [JsonProperty("prec")]
        public string Precision { get; private set; }

        [JsonProperty("symbol")]
        public string Symbol { get; private set; }

        public static SubscribeMessage ToFundings(FundingSymbols symbol)
        {
            return new SubscribeMessage
            {
                ChannelName = @"trades",
                Symbol = EnumStringValueAttribute.GetValue(symbol)
            };
        }

        public static SubscribeMessage ToFundingTicker(FundingSymbols symbol)
        {
            return new SubscribeMessage
            {
                ChannelName = @"fticker",
                Symbol = EnumStringValueAttribute.GetValue(symbol)
            };
        }

        public static SubscribeMessage ToOrderBook(TradingSymbols symbol, BooksPrecision precision, BooksFrequency frequency,
            BooksLimit limit)
        {
            return new SubscribeMessage
            {
                ChannelName = @"book",
                Symbol = EnumStringValueAttribute.GetValue(symbol),
                Precision = EnumStringValueAttribute.GetValue(precision),
                Frequency = EnumStringValueAttribute.GetValue(frequency),
                Length = ((int) limit).ToString()
            };
        }

        public static SubscribeMessage ToOrderBook(FundingSymbols symbol, BooksPrecision precision, BooksFrequency frequency,
            BooksLimit limit)
        {
            return new SubscribeMessage
            {
                ChannelName = @"book",
                Symbol = EnumStringValueAttribute.GetValue(symbol),
                Precision = EnumStringValueAttribute.GetValue(precision),
                Frequency = EnumStringValueAttribute.GetValue(frequency),
                Length = ((int) limit).ToString()
            };
        }

        public static SubscribeMessage ToRawOrderBook(TradingSymbols symbol, BooksLimit limit)
        {
            return new SubscribeMessage
            {
                ChannelName = @"book",
                Symbol = EnumStringValueAttribute.GetValue(symbol),
                Precision = EnumStringValueAttribute.GetValue(BooksPrecision.NoAggregation),
                Length = ((int) limit).ToString()
            };
        }

        public static SubscribeMessage ToRawOrderBook(FundingSymbols symbol, BooksLimit limit)
        {
            return new SubscribeMessage
            {
                ChannelName = @"book",
                Symbol = EnumStringValueAttribute.GetValue(symbol),
                Precision = EnumStringValueAttribute.GetValue(BooksPrecision.NoAggregation),
                Length = ((int) limit).ToString()
            };
        }


        public static SubscribeMessage ToTrades(TradingSymbols symbol)
        {
            return new SubscribeMessage
            {
                ChannelName = @"trades",
                Symbol = EnumStringValueAttribute.GetValue(symbol)
            };
        }


        public static SubscribeMessage ToTradingCandles(TradingSymbols symbol, CandlesTimeFrame timeFrame)
        {
            return new SubscribeMessage
            {
                ChannelName = @"candles",
                Key =
                    $"trade:{EnumStringValueAttribute.GetValue(timeFrame)}:{EnumStringValueAttribute.GetValue(symbol)}"
            };
        }

        public static SubscribeMessage ToTradingTicker(TradingSymbols symbol)
        {
            return new SubscribeMessage
            {
                ChannelName = @"ticker",
                Symbol = EnumStringValueAttribute.GetValue(symbol)
            };
        }

        /// <inheritdoc />
        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}