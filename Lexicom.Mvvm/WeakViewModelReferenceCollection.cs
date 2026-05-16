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
    public WeakViewModelReferenceCollection()
    {
        WeakViewModelRefrences = [];
        MutateLock = new Lock();

        PruneThreshold = 8;
    }

    private List<WeakReference<TViewModelImplementation>> WeakViewModelRefrences { get; }
    private Lock MutateLock { get; }
    private int PruneThreshold { get; set; }

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

        lock (MutateLock)
        {
            WeakViewModelRefrences.Add(new WeakReference<TViewModelImplementation>(viewModel));

            if (WeakViewModelRefrences.Count >= PruneThreshold)
            {
                PruneDeadReferences();
            }
        }
    }

    public IReadOnlyList<TViewModelImplementation> GetRemainingViewModels()
    {
        lock (MutateLock)
        {
            return PruneDeadReferences();
        }
    }

    //removes dead weak references in place and returns the still-live view
    //models in insertion order. callers must hold MutateLock.
    private List<TViewModelImplementation> PruneDeadReferences()
    {
        var viewModels = new List<TViewModelImplementation>();

        int writeIndex = 0;
        for (int readIndex = 0; readIndex < WeakViewModelRefrences.Count; readIndex++)
        {
            WeakReference<TViewModelImplementation> weakViewModelRefrence = WeakViewModelRefrences[readIndex];

            if (weakViewModelRefrence.TryGetTarget(out TViewModelImplementation? viewModel))
            {
                viewModels.Add(viewModel);
                WeakViewModelRefrences[writeIndex] = weakViewModelRefrence;
                writeIndex++;
            }
        }

        WeakViewModelRefrences.RemoveRange(writeIndex, WeakViewModelRefrences.Count - writeIndex);

        PruneThreshold = Math.Max(PruneThreshold, viewModels.Count * 2);

        return viewModels;
    }
}
