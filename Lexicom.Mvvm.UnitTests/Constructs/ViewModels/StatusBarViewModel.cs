using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Lexicom.Mvvm.UnitTests.Constructs.Messages;

namespace Lexicom.Mvvm.UnitTests.Constructs.ViewModels;

public partial class StatusBarViewModel : DisposableObservableObject, IAsyncRecipient<StatusMessage>, IRecipient<StatusMessage>
{
    [ObservableProperty]
    public partial int AsyncRecievedCount { get; set; }

    [ObservableProperty]
    public partial int SyncRecievedCount { get; set; }

    public Task LoadAsync()
    {
        return Task.CompletedTask;
    }

    public Task ReceiveAsync(StatusMessage message, CancellationToken cancellationToken)
    {
        AsyncRecievedCount++;

        return Task.CompletedTask;
    }

    public void Receive(StatusMessage message)
    {
        SyncRecievedCount++;
    }
}
