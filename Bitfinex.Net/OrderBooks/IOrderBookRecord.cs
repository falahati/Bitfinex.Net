using System;
using Bitfinex.Net.Enums;

namespace Bitfinex.Net.OrderBooks
{
    public interface IOrderBookRecord : IEquatable<IOrderBookRecord>
    {
        double Amount { get; }
        BookRecordType RecordType { get; }
    }
}