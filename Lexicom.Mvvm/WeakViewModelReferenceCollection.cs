using Lexicom.Mvvm.Exceptions;

namespace Lexicom.Mvvm;
public interface IWeakViewModelReferenceCollection
{
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ViewModelNotOfViewModelImplementationTypeException{TViewModelImplementation}"></exception>
    void Add(object viewModel);
}
public class WeakViewModelReferenceCollection<TViewModelImplementation> : IWeakViewModelReferenceCollection where TViewModelImplementation : class
{
    private readonly List<WeakReference<TViewModelImplementation>> _weakViewModelRefrences;

    public WeakViewModelReferenceCollection()
    {
        _weakViewModelRefrences = [];
    }

    public void Add(object viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        if (viewModel is TViewModelImplementation viewModelImplementation)
        {
            Add(viewModelImplementation);
        }
        else
        {
            throw new ViewModelNotOfViewModelImplementationTypeException<TViewModelImplementation>(viewModel);
        }
    }

    /// <exception cref="ArgumentNullException"/>
    public void Add(TViewModelImplementation viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        _weakViewModelRefrences.Add(new WeakReference<TViewModelImplementation>(viewModel));
    }

    public IReadOnlyList<TViewModelImplementation> GetRemainingViewModels()
    {
        var viewModels = new List<TViewModelImplementation>();

        foreach (var weakViewModelRefrence in _weakViewModelRefrences)
        {
            if (weakViewModelRefrence.TryGetTarget(out var viewModel))
            {
                viewModels.Add(viewModel);
            }
        }

        return viewModels;
    }
}
