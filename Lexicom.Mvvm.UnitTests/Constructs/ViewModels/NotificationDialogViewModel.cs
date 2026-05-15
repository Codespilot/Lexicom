using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Lexicom.Mvvm.UnitTests.Constructs.Messages;

namespace Lexicom.Mvvm.UnitTests.Constructs.ViewModels;

public partial class NotificationDialogViewModel : DisposableObservableObject, IRecipient<NewNotificationMessage>
{
    [ObservableProperty]
    public partial int ReceivedNotificationCount { get; set; }

    public void Receive(NewNotificationMessage message)
    {
        ReceivedNotificationCount++;
    }
}
