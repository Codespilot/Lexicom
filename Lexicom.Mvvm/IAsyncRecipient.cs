namespace Lexicom.Mvvm;

public interface IAsyncRecipient<TMessage> where TMessage : AsyncMessage
{
    Task ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}
