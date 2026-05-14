using CommunityToolkit.Mvvm.Messaging;
using Lexicom.Mvvm.Exceptions;
using Lexicom.Mvvm.Extensions;
using Lexicom.Mvvm.Support;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace Lexicom.Mvvm;
public interface IViewModelFactory
{
    TViewModel Create<TViewModel>() where TViewModel : notnull;
    TViewModel Create<TViewModel, TModel>(TModel model) where TViewModel : notnull;
    TViewModel Create<TViewModel, TModel1, TModel2>(TModel1 model1, TModel2 model2) where TViewModel : notnull;
    TViewModel Create<TViewModel, TModel1, TModel2, TModel3>(TModel1 model1, TModel2 model2, TModel3 model3) where TViewModel : notnull;
}
/// <exception cref="ArgumentNullException"/>
public class ViewModelFactory : IViewModelFactory
{
    private static MethodInfo StaticAddToWeakViewModelReferenceCollectionMethodInfo => field ??= (typeof(ViewModelFactory).GetMethod(nameof(AddToWeakViewModelReferenceCollection), BindingFlags.Static | BindingFlags.NonPublic) ?? throw new UnreachableException($"The method '{nameof(AddToWeakViewModelReferenceCollection)}' was not found."));
    private static void AddToWeakViewModelReferenceCollection<TViewModelImplementation>(IServiceProvider serviceProvider, TViewModelImplementation viewModel) where TViewModelImplementation : class
    {
        WeakViewModelReferenceCollection<TViewModelImplementation> weakViewModelReferenceCollection;
        try
        {
            weakViewModelReferenceCollection = serviceProvider.GetRequiredService<WeakViewModelReferenceCollection<TViewModelImplementation>>();
        }
        catch (InvalidOperationException e)
        {
            throw new ViewModelNotRegisteredException(typeof(TViewModelImplementation), e);
        }

        weakViewModelReferenceCollection.Add(viewModel);
    }

    private static Dictionary<Type, object> ViewModelTypeToSingletonInstance { get; } = [];

    protected readonly IServiceProvider _serviceProvider;
    protected readonly IEnumerable<IMessenger> _messengers;

    /// <exception cref="ArgumentNullException"/>
    public ViewModelFactory(IServiceProvider serviceProvider, IEnumerable<IMessenger> messengers)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(messengers);

        _serviceProvider = serviceProvider;
        _messengers = messengers;
    }

    public virtual TViewModel Create<TViewModel>() where TViewModel : notnull
    {
        return InitializeViewModel(isWithModels: false, implementationType =>
        {
            return (TViewModel)ActivatorUtilities.CreateInstance(_serviceProvider, implementationType);
        });
    }
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="SingletonViewModelAlreadyExistsException"/>
    public virtual TViewModel Create<TViewModel, TModel>(TModel model) where TViewModel : notnull
    {
        ArgumentNullException.ThrowIfNull(model);

        return InitializeViewModel(isWithModels: true, implementationType =>
        {
            return (TViewModel)ActivatorUtilities.CreateInstance(_serviceProvider, implementationType, model);
        });
    }
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="SingletonViewModelAlreadyExistsException"/>
    public virtual TViewModel Create<TViewModel, TModel1, TModel2>(TModel1 model1, TModel2 model2) where TViewModel : notnull
    {
        ArgumentNullException.ThrowIfNull(model1);
        ArgumentNullException.ThrowIfNull(model2);

        return InitializeViewModel(isWithModels: true, implementationType =>
        {
            return (TViewModel)ActivatorUtilities.CreateInstance(_serviceProvider, implementationType, model1, model2);
        });
    }
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="SingletonViewModelAlreadyExistsException"/>
    public virtual TViewModel Create<TViewModel, TModel1, TModel2, TModel3>(TModel1 model1, TModel2 model2, TModel3 model3) where TViewModel : notnull
    {
        ArgumentNullException.ThrowIfNull(model1);
        ArgumentNullException.ThrowIfNull(model2);
        ArgumentNullException.ThrowIfNull(model3);

        return InitializeViewModel(isWithModels: true, implementationType =>
        {
            return (TViewModel)ActivatorUtilities.CreateInstance(_serviceProvider, implementationType, model1, model2, model3);
        });
    }

    protected virtual Type GetViewModelImplementationType<TViewModel>() where TViewModel : notnull
    {
        var viewModelImplementationTypeAccessor = _serviceProvider.GetService<ViewModelImplementationTypeAccessor<TViewModel>>();

        Type viewModelImplementationType;
        if (viewModelImplementationTypeAccessor is not null)
        {
            viewModelImplementationType = viewModelImplementationTypeAccessor.ViewModelImplementationType;
        }
        else
        {
            viewModelImplementationType = typeof(TViewModel);
        }

        return viewModelImplementationType;
    }

    protected virtual ServiceLifetime GetViewModelServiceLifetime<TViewModel>() where TViewModel : notnull
    {
        var viewModelRegistrations = _serviceProvider.GetService<IEnumerable<ViewModelRegistration>>();
        ViewModelRegistration? viewModelRegistration = viewModelRegistrations?.FirstOrDefault(vmr => vmr.ServiceType == typeof(TViewModel));

        ServiceLifetime serviceLifetime;
        if (viewModelRegistration is not null)
        {
            serviceLifetime = viewModelRegistration.ServiceLifetime;
        }
        else
        {
            serviceLifetime = ServiceLifetime.Transient;
        }

        return serviceLifetime;
    }

    private TViewModel InitializeViewModel<TViewModel>(bool isWithModels, Func<Type, TViewModel> activateImplementationTypeDelegate) where TViewModel : notnull
    {
        Type viewModelType = typeof(TViewModel);
        Type implementationType = GetViewModelImplementationType<TViewModel>();
        ServiceLifetime serviceLifetime = GetViewModelServiceLifetime<TViewModel>();

        TViewModel? viewModel = default;
        if (serviceLifetime is ServiceLifetime.Singleton && ViewModelTypeToSingletonInstance.TryGetValue(viewModelType, out object? instance))
        {
            viewModel = (TViewModel)instance;

            if (isWithModels)
            {
                throw new SingletonViewModelAlreadyExistsException(viewModelType);
            }
        }

        if (viewModel is null)
        {
            viewModel = activateImplementationTypeDelegate.Invoke(implementationType);

            StaticAddToWeakViewModelReferenceCollectionMethodInfo
                .MakeGenericMethod(implementationType)
                .Invoke(null, [_serviceProvider, viewModel]);

            if (serviceLifetime is ServiceLifetime.Singleton)
            {
                if (ViewModelTypeToSingletonInstance.ContainsKey(viewModelType))
                {
                    //if the type is a singleton than we should have been able to access the instance above thus this should not be possible.
                    throw new UnreachableException($"The view model '{viewModelType?.FullName ?? "null"}' has already been created.");
                }

                ViewModelTypeToSingletonInstance.Add(viewModelType, viewModel);
            }

            foreach (IMessenger? messenger in _messengers)
            {
                if (messenger is not null)
                {
                    messenger?.RegisterAll(viewModel);
                    messenger?.AsyncRegisterAll(viewModel);
                }
            }
        }

        return viewModel;
    }
}
