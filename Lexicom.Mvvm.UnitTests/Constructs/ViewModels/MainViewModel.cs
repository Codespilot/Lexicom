using CommunityToolkit.Mvvm.ComponentModel;

namespace Lexicom.Mvvm.UnitTests.Constructs.ViewModels;

public partial class MainViewModel : DisposableObservableObject
{
    public MainViewModel(
        HeaderViewModel headerViewModel,
        NotificationTrayViewModel notificationTrayViewModel)
    {
        HeaderViewModel = headerViewModel;
        NotificationTrayViewModel = notificationTrayViewModel;
    }

    [ObservableProperty]
    public partial HeaderViewModel HeaderViewModel { get; set; }

    [ObservableProperty]
    public partial NotificationTrayViewModel NotificationTrayViewModel { get; set; }

    public override void Dispose()
    {
        base.Dispose();

        HeaderViewModel?.Dispose();
        NotificationTrayViewModel?.Dispose();
    }

    public async Task LoadAsync()
    {
        await HeaderViewModel.LoadAsync();
        await NotificationTrayViewModel.LoadAsync();
    }
}
