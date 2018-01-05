using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Bitfinex.Net.Realtime.ResponseMessages;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace Bitfinex.Net.Realtime
{
    public class RealtimeBitfinex : Bitfinex, IDisposable
    {
        protected const string Address = "wss://api.bitfinex.com/ws/2";
        protected const string Version = "2";
        protected readonly Dictionary<int, IRealtimeChannel> Channels = new Dictionary<int, IRealtimeChannel>();
        protected TaskCompletionSource<object> ConnectionCompletionSource;
        protected TaskCompletionSource<SubscribedMessage> OpenChannelCompletionSource;

        public RealtimeBitfinex()
        {
            WebSocket = new WebSocket(Address);
            WebSocket.Opened += WebSocketOnOpened;
            WebSocket.Closed += WebSocketOnClosed;
            WebSocket.Error += WebSocketOnError;
            WebSocket.MessageReceived += WebSocketOnMessageReceived;
        }

        public RealtimeConnectionStatus Status { get; protected set; }

        protected WebSocket WebSocket { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            WebSocket.Close();
            WebSocket.Dispose();
        }

        public async Task ConnectAsync()
        {
            lock (this)
            {
                if (Status != RealtimeConnectionStatus.Close)
                    throw new InvalidOperationException("WebSocket is already connected.");
                Status = RealtimeConnectionStatus.Connecting;
            }
            ConnectionCompletionSource = new TaskCompletionSource<object>(null);
            EventHandler<EventArgs> onOpenHandler = null;
            EventHandler<EventArgs> onErrorHandler = null;
            onOpenHandler = (sender, args) =>
            {
                // ReSharper disable AccessToModifiedClosure
                Error -= onErrorHandler;
                Connected -= onOpenHandler;
                // ReSharper restore AccessToModifiedClosure
                ConnectionCompletionSource.TrySetResult(null);
            };
            onErrorHandler = (sender, args) =>
            {
                // ReSharper disable AccessToModifiedClosure
                Error -= onErrorHandler;
                Connected -= onOpenHandler;
                // ReSharper restore AccessToModifiedClosure
                ConnectionCompletionSource.TrySetException(new Exception());
            };
            Connected += onOpenHandler;
            Connected += onErrorHandler;
            WebSocket.Open();
            await ConnectionCompletionSource.Task;
        }


        public void Disconnect()
        {
            lock (this)
            {
                if (Status == RealtimeConnectionStatus.Close)
                {
                    Debug.WriteLine("[Disconnect] {0}", "WebSocket is already disconnected.");
                    throw new InvalidOperationException("WebSocket is already disconnected.");
                }
                Status = RealtimeConnectionStatus.Close;
            }
            WebSocket.Close();
        }

        public async Task OpenChannelAsync(IRealtimeChannel channel, CancellationToken cancellationToken)
        {
            var subscriptionMessage = channel.GetSubscriptionMessage();
            var myTask = new TaskCompletionSource<SubscribedMessage>();
            while (!cancellationToken.IsCancellationRequested)
            {
                Task<SubscribedMessage> task;
                lock (this)
                {
                    if (OpenChannelCompletionSource == null)
                    {
                        OpenChannelCompletionSource = myTask;
                        break;
                    }
                    task = OpenChannelCompletionSource.Task;
                }
                try
                {
                    await task;
                }
                catch
                {
                    // ignore
                }
            }

            // SubscribeMessage to the event
            EventHandler<MessageReceivedEventArgs> messagEventHandler = null;
            messagEventHandler = (sender, args) =>
            {
                RealtimeMessage message;
                if ((message = RealtimeMessage.Deserialize(args.Message)) != null)
                    if (message is SubscribedMessage)
                    {
                        WebSocket.MessageReceived -= messagEventHandler;
                        lock (this)
                        {
                            myTask.TrySetResult(message as SubscribedMessage);
                        }
                    }
                    else if (message is ErrorMessage)
                    {
                        WebSocket.MessageReceived -= messagEventHandler;
                        lock (this)
                        {
                            myTask.TrySetException(new Exception((message as ErrorMessage).Message));
                        }
                    }
            };
            WebSocket.MessageReceived += messagEventHandler;

            // Cancellation token
            cancellationToken.Register(() =>
            {
                lock (this)
                {
                    myTask.TrySetCanceled();
                }
            });

            // Send subscription message
            WebSocket.Send(subscriptionMessage.Serialize());

            channel.OnChannelSubscribed(await myTask.Task);

            lock (this)
            {
                OpenChannelCompletionSource = null;
                if (Channels.ContainsKey(channel.ChannelId))
                    Channels[channel.ChannelId] = channel;
                else
                    Channels.Add(channel.ChannelId, channel);
            }
        }

        protected void Reconnect()
        {
            lock (this)
            {
                if (Status == RealtimeConnectionStatus.Close)
                    return;
                Status = RealtimeConnectionStatus.Close;
            }
            // Fire and forget
            Task.Run(async () =>
            {
                await Task.Delay(300);
                try
                {
                    await ConnectAsync();
                }
                catch
                {
                    // ignored
                }
            });
        }

        protected void WebSocketOnClosed(object sender, EventArgs eventArgs)
        {
            Reconnect();
        }

        protected void WebSocketOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            try
            {
                Disconnect();
            }
            catch
            {
                // ignored
            }
            OnError(errorEventArgs.Exception);
        }

        protected void WebSocketOnMessageReceived(object sender, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            OnRawMessageReceived(messageReceivedEventArgs.Message);
            RealtimeChannelResponse response;
            RealtimeMessage message;
            if ((response = RealtimeChannelResponse.Deserialize(messageReceivedEventArgs.Message)) != null)
            {
                IRealtimeChannel realtimeChannel = null;
                lock (this)
                {
                    if (Channels.ContainsKey(response.ChannelId))
                        realtimeChannel = Channels[response.ChannelId];
                }
                realtimeChannel?.OnChannelResponse(response);
                OnChannelUpdated(realtimeChannel);
            }
            else if ((message = RealtimeMessage.Deserialize(messageReceivedEventArgs.Message)) != null)
            {
                OnMessageReceived(message);
            }
        }

        protected void WebSocketOnOpened(object sender, EventArgs eventArgs)
        {
            lock (this)
            {
                Status = RealtimeConnectionStatus.Open;
            }
            OnConnect();
        }

        #region Events

        public event EventHandler<EventArgs> ChannelUpdated;
        public event EventHandler<EventArgs> RawMessageReceived;
        public event EventHandler<EventArgs> Connected;
        public event EventHandler<EventArgs> Disconnected;
        public event EventHandler<EventArgs> Error;
        public event EventHandler<EventArgs> MessageReceived;

        protected void OnChannelUpdated(IRealtimeChannel channel)
        {
            ChannelUpdated?.Invoke(this, new EventArgs());
        }

        protected void OnMessageReceived(RealtimeMessage message)
        {
            MessageReceived?.Invoke(this, new EventArgs());
        }

        protected void OnRawMessageReceived(string message)
        {
            RawMessageReceived?.Invoke(this, new EventArgs());
        }

        protected void OnDisconnected()
        {
            Disconnected?.Invoke(this, new EventArgs());
        }

        protected void OnConnect()
        {
            Connected?.Invoke(this, new EventArgs());
        }

        protected void OnError(Exception exception)
        {
            Error?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}