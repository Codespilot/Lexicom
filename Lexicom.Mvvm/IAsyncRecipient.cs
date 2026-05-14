using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicom.Mvvm;

public interface IAsyncRecipient<TMessage> where TMessage : AsyncMessage
{
    Task ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}
