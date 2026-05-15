using CommunityToolkit.Mvvm.Messaging;
using Lexicom.Mvvm.Extensions;
using Lexicom.Mvvm.UnitTests.Constructs.Messages;
using Lexicom.Mvvm.UnitTests.Constructs.Services;
using Lexicom.Mvvm.UnitTests.Constructs.ViewModels;
using Lexicom.Testing.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Lexicom.Mvvm.UnitTests.Tests;

public class AsyncMessengerTests
{
    [Fact]
    public async Task Temp()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomMvvm(mvvm =>
        {
            mvvm.AddViewModel<HeaderViewModel>();
            mvvm.AddViewModel<MainViewModel>();
            mvvm.AddViewModel<ProfileViewModel>();
        });

        var notificationService = new NotificationService();

        ita.AddSingleton<IAccountService, AccountService>();
        ita.AddSingleton<INotificationService>(notificationService);

        var vm = ita.Make<MainViewModel>();
        var messenger = ita.Make<IMessenger>();

        //act
        await vm.LoadAsync();

        int? initalNotificationCount = vm.HeaderViewModel?.ProfileViewModel?.NotificationsCount;

        notificationService.Count += 3;

        await messenger.SendAsync(new NewNotificationMessage());

        int? laterNotificationCount = vm.HeaderViewModel?.ProfileViewModel?.NotificationsCount;

        //assert
        Assert.NotNull(initalNotificationCount);
        Assert.NotNull(laterNotificationCount);

        Assert.Equal(5, initalNotificationCount.Value);
        Assert.Equal(8, laterNotificationCount.Value);
    }
}
