using System;

namespace Bitfinex.Net.OrderBooks
{
    public class TradingBookRecord : IEquatable<TradingBookRecord>
    {
        public TradingBookRecord(double price, double amount, uint count)
        {
            if (count == 0)
                throw new ArgumentException("Parameter can not be zero.", nameof(count));
            Price = price;
            Amount = Math.Abs(amount);
            Count = count;
        }

        public TradingBookRecord()
        {
        }

        public double Amount { get; private set; }
        public uint Count { get; private set; }
        public double Price { get; }

        /// <inheritdoc />
        public bool Equals(TradingBookRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Math.Abs(other.Price - Price) < double.Epsilon;
        }

        /// <inheritdoc />
        public static bool operator ==(TradingBookRecord left, TradingBookRecord right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(TradingBookRecord left, TradingBookRecord right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TradingBookRecord) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Price.GetHashCode();
        }

        internal void Update(double amount, uint count)
        {
            if (count == 0)
                throw new ArgumentException("Parameter can not be zero.", nameof(count));
            Amount = Math.Abs(amount);
            Count = count;
        }
    }
}