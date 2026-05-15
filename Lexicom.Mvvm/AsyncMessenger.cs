using CommunityToolkit.Mvvm.Messaging;

namespace Lexicom.Mvvm;

public class AsyncMessenger : IMessenger
{
    private readonly WeakReferenceMessenger _messenger;
    private readonly IMessengerScheduler _messengerScheduler;

    /// <exception cref="ArgumentNullException"></exception>
    public AsyncMessenger(
        WeakReferenceMessenger messenger, 
        IMessengerScheduler messengerScheduler)
    {
        ArgumentNullException.ThrowIfNull(messenger);
        ArgumentNullException.ThrowIfNull(messengerScheduler);

        _messenger = messenger;
        _messengerScheduler = messengerScheduler;
    }

    public void Cleanup()
    {
        _messenger.Cleanup();
    }

    public void Reset()
    {
        _messenger.Reset();
    }

    /// <exception cref="ArgumentNullException"></exception>
    public bool IsRegistered<TMessage, TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
    {
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(token);

        return _messenger.IsRegistered<TMessage, TToken>(recipient, token);
    }

    /// <exception cref="ArgumentNullException"></exception>
    public void Register<TRecipient, TMessage, TToken>(TRecipient recipient, TToken token, MessageHandler<TRecipient, TMessage> handler) where TRecipient : class where TMessage : class where TToken : IEquatable<TToken>
    {
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(handler);

        _messenger.Register<TRecipient, TMessage, TToken>(recipient, token, (r, message) =>
        {
            if (recipient is DisposableObservableObject disposable && disposable.IsDisposed)
            {
                return;
            }

            handler.Invoke(recipient, message);
        });
    }

    public void AsyncRegister<TMessage>(IAsyncRecipient<TMessage> recipient) where TMessage : AsyncMessage
    {
        IMessengerExtensions.Register<TMessage>(this, recipient, (r, message) =>
        {
            if (r is IAsyncRecipient<TMessage> asyncRecipient)
            {
                if (asyncRecipient is DisposableObservableObject disposable && disposable.IsDisposed)
                {
                    return;
                }

                //normally you would execute a send here in the register handler function
                //however since we want to handle a async call we need to be able to await
                //it which we cant do here since this is a sync method. so we use a reply
                //to simply bundle the recipient and message together for sending later in
                //the calling SendAsync method which is async itself.

                var reply = new AsyncMessageReply<TMessage>(message, asyncRecipient);

                message.Reply(reply);
            }
        });
    }

    /// <exception cref="ArgumentNullException"></exception>
    public TMessage Send<TMessage, TToken>(TMessage message, TToken token) where TMessage : class where TToken : IEquatable<TToken>
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(token);

        return _messenger.Send(message, token);
    }

    /// <exception cref="ArgumentNullException"></exception>
    public async Task SendAsync<TMessage>(TMessage message, AsyncMessageAwaitStrategy asyncMessageAwaitStrategy, CancellationToken cancellationToken = default) where TMessage : AsyncMessage
    {
        ArgumentNullException.ThrowIfNull(message);

        //this will not actually send anything but instead bundle the
        //recipient and message together so we can do the actual send below
        IMessengerExtensions.Send(this, message);

        if (asyncMessageAwaitStrategy == AsyncMessageAwaitStrategy.ForeachAwait)
        {
            foreach (IAsyncMessageReply reply in message.Responses)
            {
                await reply.SendAsync(cancellationToken);
            }
        }
        else
        {
            var tasks = new HashSet<Task>();
            foreach (IAsyncMessageReply reply in message.Responses)
            {
                Task sendTask = reply.SendAsync(cancellationToken);

                tasks.Add(sendTask);
            }

            await Task.WhenAll(tasks);
        }
    }

    public async Task ScheduleAsync<TMessage>(TMessage message, ScheduleMessagePriority scheduleMessagePriority, AsyncMessageAwaitStrategy asyncMessageAwaitStrategy, CancellationToken cancellationToken = default) where TMessage : AsyncMessage
    {
        ArgumentNullException.ThrowIfNull(message);

        await _messengerScheduler.DispatchAsync(scheduleMessagePriority, async () =>
        {
            await SendAsync(message, asyncMessageAwaitStrategy, cancellationToken);
        });
    }

    /// <exception cref="ArgumentNullException"></exception>
    public void Unregister<TMessage, TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>
    {
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(token);

        _messenger.Unregister<TMessage, TToken>(recipient, token);
    }

    /// <exception cref="ArgumentNullException"></exception>
    public void UnregisterAll(object recipient)
    {
        ArgumentNullException.ThrowIfNull(recipient);

        _messenger.UnregisterAll(recipient);
    }

    /// <exception cref="ArgumentNullException"></exception>
    public void UnregisterAll<TToken>(object recipient, TToken token) where TToken : IEquatable<TToken>
    {
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(token);

        _messenger.UnregisterAll(recipient, token);
    }
}
