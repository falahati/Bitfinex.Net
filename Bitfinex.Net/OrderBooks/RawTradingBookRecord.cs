using System;
using System.Linq;
using Bitfinex.Net.Enums;
using Newtonsoft.Json;

namespace Bitfinex.Net.OrderBooks
{
    public class RawTradingBookRecord : IOrderBookRecord, IEquatable<RawTradingBookRecord>
    {
        protected RawTradingBookRecord(uint orderId, double price, double amount)
        {
            OrderId = orderId;
            Price = price;
            RecordType = Amount > 0 ? BookRecordType.Bid : BookRecordType.Ask;
            Amount = Math.Abs(amount);
        }

        public uint OrderId { get; }
        public double Price { get; private set; }

        /// <inheritdoc />
        public bool Equals(IOrderBookRecord other)
        {
            return Equals(other as RawTradingBookRecord);
        }

        /// <inheritdoc />
        public bool Equals(RawTradingBookRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Math.Abs(other.Amount - Amount) < double.Epsilon) &&
                   (OrderId == other.OrderId);
        }

        public double Amount { get; }

        public BookRecordType RecordType { get; }

        public static RawTradingBookRecord Deserialize(string serialized)
        {
            if (!serialized.StartsWith("["))
                return null;
            var record = JsonConvert.DeserializeObject<object[]>(serialized);
            if ((record.Length != 3) || record.Any(o => o.GetType().IsClass))
                return null;
            return new RawTradingBookRecord(
                uint.Parse(record[0].ToString()),
                double.Parse(record[1].ToString()),
                double.Parse(record[2].ToString()));
        }

        /// <inheritdoc />
        public static bool operator ==(RawTradingBookRecord left, RawTradingBookRecord right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(RawTradingBookRecord left, RawTradingBookRecord right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RawTradingBookRecord) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Amount.GetHashCode();
                hashCode = (hashCode*397) ^ (int) OrderId;
                hashCode = (hashCode*397) ^ (int) RecordType;
                return hashCode;
            }
        }

        public void Update(RawTradingBookRecord record)
        {
            if (Equals(record))
                Price = record.Price;
        }
    }
}