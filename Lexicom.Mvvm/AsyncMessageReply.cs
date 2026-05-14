namespace Lexicom.Mvvm;

public interface IAsyncMessageReply
{
    Task SendAsync(CancellationToken cancellationToken);
}
public class AsyncMessageReply<TMessage> : IAsyncMessageReply where TMessage : AsyncMessage
{
    /// <exception cref="ArgumentNullException"></exception>
    public AsyncMessageReply(
        TMessage messsage,
        IAsyncRecipient<TMessage> recipient)
    {
        ArgumentNullException.ThrowIfNull(messsage);
        ArgumentNullException.ThrowIfNull(recipient);

        Messsage = messsage;
        Recipient = recipient;
    }

    public TMessage Messsage { get; }
    public IAsyncRecipient<TMessage> Recipient { get; }

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        await Recipient.ReceiveAsync(Messsage, cancellationToken);
    }
}
