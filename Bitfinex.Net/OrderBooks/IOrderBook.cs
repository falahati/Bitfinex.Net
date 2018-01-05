namespace Bitfinex.Net.OrderBooks
{
    public interface IOrderBook<T> : IChannel where T : IOrderBookRecord
    {
        T[] Asks { get; }

        T[] Bids { get; }
    }
}