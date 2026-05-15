using CommunityToolkit.Mvvm.ComponentModel;
using Lexicom.Mvvm.UnitTests.Constructs.Messages;
using Lexicom.Mvvm.UnitTests.Constructs.Models;
using Lexicom.Mvvm.UnitTests.Constructs.Services;

namespace Lexicom.Mvvm.UnitTests.Constructs.ViewModels;

public partial class NotificationTrayViewModel : DisposableObservableObject, IAsyncRecipient<NewNotificationMessage>
{
    private readonly IAccountService _accountService;
    private readonly INotificationService _notificationService;

    public NotificationTrayViewModel(
        NotificationDialogViewModel notificationDialogViewModel,
        IAccountService accountService,
        INotificationService notificationService)
    {
        _accountService = accountService;
        _notificationService = notificationService;

        NotificationDialogViewModel = notificationDialogViewModel;
    }

    [ObservableProperty]
    public partial int NotificationsCount { get; set; }

    [ObservableProperty]
    public partial bool IsReceivedNotification { get; set; }

    [ObservableProperty]
    public partial NotificationDialogViewModel NotificationDialogViewModel { get; set; }

    public async Task LoadAsync()
    {
        await UpdateNotificationsCountAsync();
    }

    public async Task ReceiveAsync(NewNotificationMessage message, CancellationToken cancellationToken)
    {
        IsReceivedNotification = true;

        await UpdateNotificationsCountAsync();
    }

    private async Task UpdateNotificationsCountAsync()
    {
        Account account = await _accountService.GetLoggedInAccountAsync();

        NotificationsCount = await _notificationService.GetNotificationsCountAsync(account.ProfileId);
    }
}
