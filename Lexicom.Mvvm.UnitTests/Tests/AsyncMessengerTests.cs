using CommunityToolkit.Mvvm.Messaging;
using Lexicom.Mvvm.Extensions;
using Lexicom.Mvvm.For.Testing.Extensions;
using Lexicom.Mvvm.UnitTests.Constructs.Messages;
using Lexicom.Mvvm.UnitTests.Constructs.Services;
using Lexicom.Mvvm.UnitTests.Constructs.ViewModels;
using Lexicom.Supports.Testing.Extensions;
using Lexicom.Testing.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Lexicom.Mvvm.UnitTests.Tests;

public class AsyncMessengerTests
{
    [Fact]
    public async Task Sending_Async_Message_Is_Recived_By_ViewModels()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        ita.Lexicom(l =>
        {
            l.AddMvvm(mvvm =>
            {
                mvvm.AddViewModel<HeaderViewModel>();
                mvvm.AddViewModel<MainViewModel>();
                //mvvm.AddViewModel<NotificationDialogViewModel>();
                //mvvm.AddViewModel<NotificationTrayViewModel>();
                mvvm.AddViewModel<ProfileViewModel>();
            });
        });

        var notificationService = new NotificationService();

        ita.AddSingleton<IAccountService, AccountService>();
        ita.AddSingleton<INotificationService>(notificationService);

        var vm = ita.Make<MainViewModel>();
        var messenger = ita.Make<IMessenger>();

        //act
        await vm.LoadAsync();

        int? initalNotificationProfileCount = vm.HeaderViewModel?.ProfileViewModel?.NotificationsCount;
        int? initalNotificationTrayCount = vm.NotificationTrayViewModel?.NotificationsCount;

        notificationService.Count += 3;

        await messenger.SendAsync(new NewNotificationMessage(), TestContext.Current.CancellationToken);

        int? laterNotificationProfileCount = vm.HeaderViewModel?.ProfileViewModel?.NotificationsCount;
        int? laterNotificationTrayCount = vm.NotificationTrayViewModel?.NotificationsCount;

        //assert
        Assert.NotNull(initalNotificationProfileCount);
        Assert.NotNull(initalNotificationTrayCount);
        Assert.NotNull(laterNotificationProfileCount);
        Assert.NotNull(laterNotificationTrayCount);

        Assert.Equal(5, initalNotificationProfileCount.Value);
        Assert.Equal(5, initalNotificationTrayCount.Value);
        Assert.Equal(8, laterNotificationProfileCount.Value);
        Assert.Equal(8, laterNotificationTrayCount.Value);
    }

    [Fact]
    public async Task Async_Message_Is_Recived_By_Async_Recipents_And_Sync_Recipents()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        ita.Lexicom(l =>
        {
            l.AddMvvm(mvvm =>
            {
                mvvm.AddViewModel<HeaderViewModel>();
                mvvm.AddViewModel<MainViewModel>();
                mvvm.AddViewModel<NotificationDialogViewModel>();
                mvvm.AddViewModel<NotificationTrayViewModel>();
                mvvm.AddViewModel<ProfileViewModel>();
            });
        });

        var notificationService = new NotificationService();

        ita.AddSingleton<IAccountService, AccountService>();
        ita.AddSingleton<INotificationService>(notificationService);

        var vm = ita.Make<MainViewModel>();
        var messenger = ita.Make<IMessenger>();

        //act
        await vm.LoadAsync();

        bool initalIsReceivedNotificationTray = vm.NotificationTrayViewModel.IsReceivedNotification;
        bool initalIsReceivedNotificationDialog = vm.NotificationTrayViewModel.NotificationDialogViewModel.IsReceivedNotification;

        await messenger.SendAsync(new NewNotificationMessage(), TestContext.Current.CancellationToken);

        bool laterIsReceivedNotificationTray = vm.NotificationTrayViewModel.IsReceivedNotification;
        bool laterIsReceivedNotificationDialog = vm.NotificationTrayViewModel.NotificationDialogViewModel.IsReceivedNotification;

        //assert
        Assert.False(initalIsReceivedNotificationTray);
        Assert.False(initalIsReceivedNotificationDialog);

        Assert.True(laterIsReceivedNotificationTray);
        Assert.True(laterIsReceivedNotificationDialog);
    }
}
