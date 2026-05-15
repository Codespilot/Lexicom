using CommunityToolkit.Mvvm.ComponentModel;

namespace Lexicom.Mvvm.For.Testing.UnitTests.Constructs;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel(SubViewModel subViewModel)
    {
        SubViewModel = subViewModel;
    }

    [ObservableProperty]
    public partial SubViewModel SubViewModel { get; set; }
}
