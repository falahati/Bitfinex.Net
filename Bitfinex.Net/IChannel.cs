namespace Bitfinex.Net
{
    public interface IChannel
    {
        void OnChannelResponse(ChannelResponse response);
    }
}