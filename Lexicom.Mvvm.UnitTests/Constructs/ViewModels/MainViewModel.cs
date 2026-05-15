using CommunityToolkit.Mvvm.ComponentModel;

namespace Lexicom.Mvvm.UnitTests.Constructs.ViewModels;

public partial class MainViewModel : DisposableObservableObject
{
    public MainViewModel(HeaderViewModel headerViewModel)
    {
        HeaderViewModel = headerViewModel;
    }

    [ObservableProperty]
    public partial HeaderViewModel HeaderViewModel { get; set; }

    public override void Dispose()
    {
        base.Dispose();

        HeaderViewModel?.Dispose();
    }

    public async Task LoadAsync()
    {
        await HeaderViewModel.LoadAsync();
    }
}
