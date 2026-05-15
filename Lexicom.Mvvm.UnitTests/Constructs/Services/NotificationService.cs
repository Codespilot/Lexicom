namespace Lexicom.Mvvm.UnitTests.Constructs.Services;

public interface INotificationService
{
    Task<int> GetNotificationsCountAsync(Guid profileId);
}
public class NotificationService : INotificationService
{
    public Task<int> GetNotificationsCountAsync(Guid profileId)
    {
        return Task.FromResult(55);
    }
}
