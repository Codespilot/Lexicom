using Lexicom.Mvvm.Extensions;
using Lexicom.Mvvm.UnitTests.Constructs.ViewModels;
using Lexicom.Testing.DependencyInjection;

namespace Lexicom.Mvvm.UnitTests.Tests;

public class AsyncMessengerTests
{
    public async Task Temp()
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomMvvm(mvvm =>
        {
            mvvm.AddViewModel<HeaderViewModel>();
            mvvm.AddViewModel<MainViewModel>();
            mvvm.AddViewModel<ProfileViewModel>();
        });

        var vm = ita.Make<MainViewModel>();

        //act
        await vm.LoadAsync();
    }
}
