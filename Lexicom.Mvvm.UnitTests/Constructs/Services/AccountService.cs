using Lexicom.Mvvm.UnitTests.Constructs.Models;

namespace Lexicom.Mvvm.UnitTests.Constructs.Services;

public interface IAccountService
{
    Task<Account> GetLoggedInAccountAsync();
    Task<string> GetProfileNameAsync(Guid profileId);
}
public class AccountService : IAccountService
{
    public Task<Account> GetLoggedInAccountAsync()
    {
        var account = new Account
        {
            ProfileId = Guid.NewGuid(),
        };

        return Task.FromResult(account);
    }

    public Task<string> GetProfileNameAsync(Guid profileId)
    {
        return Task.FromResult("Alex");
    }
}
