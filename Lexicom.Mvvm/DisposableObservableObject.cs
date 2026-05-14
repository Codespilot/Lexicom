using CommunityToolkit.Mvvm.ComponentModel;

namespace Lexicom.Mvvm;

public class DisposableObservableObject : ObservableObject, IDisposable
{
    public bool IsDisposed { get; private set; }

    public virtual void Dispose()
    {
        IsDisposed = true;
    }
}
