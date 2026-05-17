using Lexicom.Mvvm.Exceptions;
using Lexicom.Mvvm.Extensions;
using Lexicom.Mvvm.For.Testing.Extensions;
using Lexicom.Mvvm.UnitTests.Constructs.ViewModels;
using Lexicom.Supports.Testing.Extensions;
using Lexicom.Testing.DependencyInjection;

namespace Lexicom.Mvvm.UnitTests.Tests;

public class ViewModelFactoryTests
{
    [Fact]
    public void Fail_To_Create_ViewModel_From_Factory_When_Its_Not_Registered()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        ita.TestLexicom(l =>
        {
            l.AddMvvm();
        });

        //assert
        Assert.Throws<ViewModelNotRegisteredException>(() =>
        {
            var viewModelFactory = ita.Make<IViewModelFactory>();

            //act
            viewModelFactory.Create<HeaderViewModel>();
        });
    }

    [Fact]
    public void Fail_To_Create_ViewModel_From_Factory_When_It_Injects_ViewModel_Thats_Not_Registered()
    {
        var ita = new IntegrationTestAssistant();

        ita.TestLexicom(l =>
        {
            l.AddMvvm(mvvm =>
            {
                mvvm.AddViewModel<NotificationTrayViewModel>();
            });
        });

        //assert
        Assert.Throws<ViewModelNotRegisteredException>(() =>
        {
            var viewModelFactory = ita.Make<IViewModelFactory>();

            //act
            viewModelFactory.Create<NotificationTrayViewModel>();
        }, e =>
        {
            if (!e.Message.Contains(nameof(NotificationDialogViewModel), StringComparison.OrdinalIgnoreCase) || e.Message.Contains(nameof(NotificationTrayViewModel), StringComparison.OrdinalIgnoreCase))
            {
                return "The expected exception did not have the expected message.";
            }

            return null;
        });
    }
}
