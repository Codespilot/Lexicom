using CommunityToolkit.Mvvm.Messaging;
using Lexicom.Testing.DependencyInjection;

namespace Lexicom.Mvvm.For.Testing;

public class TestViewModelFactory : ViewModelFactory
{
    private readonly IIntegrationTestAssistant _integrationTestAssistant;

    /// <exception cref="ArgumentNullException"/>
    public TestViewModelFactory(
        IServiceProvider serviceProvider,
        IEnumerable<IMessenger> messengers,
        IIntegrationTestAssistant integrationTestAssistant)
        : base(
            serviceProvider,
            messengers)
    {
        _integrationTestAssistant = integrationTestAssistant;
    }

    //protected override TViewModel CreateInstance<TViewModel>(Type implementationType)
    //{
    //    if (_integrationTestAssistant.IsMakingInstance)
    //    {
    //        return base.CreateInstance<TViewModel>(implementationType);
    //    }

    //    return (TViewModel)_integrationTestAssistant.Make(implementationType);
    //}
    //protected override TViewModel CreateInstance<TViewModel, TModel>(Type implementationType, TModel model)
    //{
    //    ArgumentNullException.ThrowIfNull(model);

    //    if (_integrationTestAssistant.IsMakingInstance)
    //    {
    //        return base.CreateInstance<TViewModel, TModel>(implementationType, model);
    //    }

    //    return (TViewModel)_integrationTestAssistant.Make(implementationType, model);
    //}
    //protected override TViewModel CreateInstance<TViewModel, TModel1, TModel2>(Type implementationType, TModel1 model1, TModel2 model2)
    //{
    //    ArgumentNullException.ThrowIfNull(model1);
    //    ArgumentNullException.ThrowIfNull(model2);

    //    if (_integrationTestAssistant.IsMakingInstance)
    //    {
    //        return base.CreateInstance<TViewModel, TModel1, TModel2>(implementationType, model1, model2);
    //    }

    //    return (TViewModel)_integrationTestAssistant.Make(implementationType, model1, model2);
    //}
    //protected override TViewModel CreateInstance<TViewModel, TModel1, TModel2, TModel3>(Type implementationType, TModel1 model1, TModel2 model2, TModel3 model3)
    //{
    //    ArgumentNullException.ThrowIfNull(model1);
    //    ArgumentNullException.ThrowIfNull(model2);
    //    ArgumentNullException.ThrowIfNull(model3);

    //    if (_integrationTestAssistant.IsMakingInstance)
    //    {
    //        return base.CreateInstance<TViewModel, TModel1, TModel2, TModel3>(implementationType, model1, model2, model3);
    //    }

    //    return (TViewModel)_integrationTestAssistant.Make(implementationType, model1, model2, model3);
    //}
}
