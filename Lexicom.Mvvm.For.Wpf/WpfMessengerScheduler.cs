using System.Windows;
using System.Windows.Threading;

namespace Lexicom.Mvvm.For.Wpf;

public class WpfMessengerScheduler : IMessengerScheduler
{
    public Task DispatchAsync(ScheduleMessagePriority priority, Func<Task>? dispatchedDelegate)
    {
        DispatcherPriority dispatcherPriority;
        if (priority == ScheduleMessagePriority.ApplicationIdle)
        {
            dispatcherPriority = DispatcherPriority.ApplicationIdle;
        }
        else
        {
            dispatcherPriority = DispatcherPriority.Normal;
        }

        Application.Current.Dispatcher.BeginInvoke(dispatcherPriority, dispatchedDelegate);

        return Task.CompletedTask;
    }
}
