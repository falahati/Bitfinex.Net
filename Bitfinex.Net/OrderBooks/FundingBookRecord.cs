using System;

namespace Bitfinex.Net.OrderBooks
{
    public class FundingBookRecord : IEquatable<FundingBookRecord>
    {
        public FundingBookRecord(double rate, double period, double amount, uint count)
        {
            if (count == 0)
                throw new ArgumentException("Parameter can not be zero.", nameof(count));
            Rate = rate;
            Period = period;
            Amount = Math.Abs(amount);
            Count = count;
        }

        public FundingBookRecord()
        {
        }

        public double Amount { get; private set; }
        public uint Count { get; private set; }
        public double Period { get; }
        public double Rate { get; }

        /// <inheritdoc />
        public bool Equals(FundingBookRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Math.Abs(other.Rate - Rate) < double.Epsilon) &&
                   (Math.Abs(other.Period - Period) < double.Epsilon);
        }

        /// <inheritdoc />
        public static bool operator ==(FundingBookRecord left, FundingBookRecord right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(FundingBookRecord left, FundingBookRecord right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FundingBookRecord) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Period.GetHashCode()*397) ^ Rate.GetHashCode();
            }
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