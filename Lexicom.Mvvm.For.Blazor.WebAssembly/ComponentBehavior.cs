using CommunityToolkit.Mvvm.Input;
using Lexicom.Mvvm.For.Blazor.WebAssembly.Exceptions;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace Lexicom.Mvvm.For.Blazor.WebAssembly;
public class ComponentBehavior<TViewModel> where TViewModel : INotifyPropertyChanged
{
    private readonly IMvvmComponent<TViewModel> _mvvmComponent;

    /// <exception cref="ArgumentNullException"/>
    public ComponentBehavior(IMvvmComponent<TViewModel> mvvmComponent)
    {
        ArgumentNullException.ThrowIfNull(mvvmComponent);

        _mvvmComponent = mvvmComponent;

        NotifyCollectionChangedProperties = [];
    }

    private PropertyInfo? LoadedCommand { get; set; }
    private PropertyInfo? RenderedCommand { get; set; }
    private List<PropertyInfo> NotifyCollectionChangedProperties { get; }

    public async Task InitializeAsync()
    {
        await ExecuteCommandAsync(Constants.COMMAND_LOADED, "OnInitializedAsync()", LoadedCommand);
    }

    public async Task AfterRenderAsync(bool firstRender)
    {
        await ExecuteCommandAsync(Constants.COMMAND_RENDERED, "OnAfterRenderAsync()", RenderedCommand, firstRender);
    }

    public void DisposeViewModel()
    {
        if (_mvvmComponent.ViewModel is not null)
        {
            UnSubscribeToCollectionChanged();

            _mvvmComponent.ViewModel.PropertyChanged -= OnPropertyChanged;
        }
    }

    public void SubmitViewModel()
    {
        if (_mvvmComponent.ViewModel is not null)
        {
            CacheViewModelProperties();

            _mvvmComponent.ViewModel.PropertyChanged += OnPropertyChanged;

            SubscribeToCollectionChanged();
        }
    }

    private async Task ExecuteCommandAsync(string commmandName, string from, PropertyInfo? commandProperty, object? parameter = null)
    {
        if (_mvvmComponent.ViewModel is null)
        {
            throw new ViewModelIsNullException(_mvvmComponent);
        }

        if (commandProperty is not null)
        {
            object? commandPropertyValue = commandProperty.GetValue(_mvvmComponent.ViewModel);

            if (commandPropertyValue is IAsyncRelayCommand asyncCommand)
            {
                await asyncCommand.ExecuteAsync(parameter);
            }
            else if (commandPropertyValue is ICommand command)
            {
                command.Execute(parameter);
            }
            else
            {
                throw new CommandNotValidException(commmandName, from, _mvvmComponent, _mvvmComponent.ViewModel);
            }
        }
    }

    private void CacheViewModelProperties()
    {
        if (_mvvmComponent.ViewModel is not null)
        {
            PropertyInfo[] properties = _mvvmComponent.ViewModel
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.Name is Constants.COMMAND_LOADED)
                {
                    LoadedCommand = property;
                }
                else if (property.Name is Constants.COMMAND_RENDERED)
                {
                    RenderedCommand = property;
                }
                else if (property.PropertyType.IsAssignableTo(typeof(INotifyCollectionChanged)))
                {
                    NotifyCollectionChangedProperties.Add(property);
                }
            }
        }
    }

    private void UnSubscribeToCollectionChanged()
    {
        if (_mvvmComponent is not null && _mvvmComponent.ViewModel is not null && NotifyCollectionChangedProperties is not null)
        {
            foreach (PropertyInfo notifyCollectionChangedProperty in NotifyCollectionChangedProperties)
            {
                if (notifyCollectionChangedProperty is not null)
                {
                    object? value = notifyCollectionChangedProperty.GetValue(_mvvmComponent.ViewModel);

                    if (value is not null and INotifyCollectionChanged notifyCollectionChanged)
                    {
                        notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
                    }
                }
            }
        }
    }

    private void SubscribeToCollectionChanged()
    {
        if (_mvvmComponent is not null && _mvvmComponent.ViewModel is not null && NotifyCollectionChangedProperties is not null)
        {
            foreach (PropertyInfo notifyCollectionChangedProperty in NotifyCollectionChangedProperties)
            {
                if (notifyCollectionChangedProperty is not null)
                {
                    object? value = notifyCollectionChangedProperty.GetValue(_mvvmComponent.ViewModel);

                    if (value is not null and INotifyCollectionChanged notifyCollectionChanged)
                    {
                        notifyCollectionChanged.CollectionChanged += OnCollectionChanged;
                    }
                }
            }
        }
    }

    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_mvvmComponent is not null)
        {
            if (_mvvmComponent.ViewModel is not null)
            {
                UnSubscribeToCollectionChanged();
                SubscribeToCollectionChanged();
            }

            await _mvvmComponent.InvokeStateChangeAsync();
        }
    }

    private async void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_mvvmComponent is not null)
        {
            await _mvvmComponent.InvokeStateChangeAsync();
        }
    }
}
