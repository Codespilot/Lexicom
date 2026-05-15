using Lexicom.DependencyInjection.Amenities.Extensions;
using Lexicom.EntityFramework.Identity.Extensions;
using Lexicom.EntityFramework.Identity.UnitTesting;
using Lexicom.Testing.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lexicom.EntityFramework.Identity.For.UnitTesting.Extensions;

public static class IntegrationTestAssistantExtensions
{
    /// <exception cref="ArgumentNullException"/>
    public static void AddLexicomEntityFrameworkIdentityForTesting<TDbContext>(this IntegrationTestAssistant integrationTestAssistant, IdentityOptions? identityOptions = null) where TDbContext : IdentityDbContext
    {
        ArgumentNullException.ThrowIfNull(integrationTestAssistant);

        PreTestIdentityConfiguration(integrationTestAssistant, identityOptions);

        integrationTestAssistant.AddLexicomEntityFrameworkIdentity<TDbContext>();

        PostTestIdentityConfiguration(integrationTestAssistant);
    }
    /// <exception cref="ArgumentNullException"/>
    public static void AddLexicomEntityFrameworkIdentityForTesting<TDbContext, TUser, TRole, TKey>(this IntegrationTestAssistant integrationTestAssistant, IdentityOptions? identityOptions = null)
        where TDbContext : IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        ArgumentNullException.ThrowIfNull(integrationTestAssistant);

        PreTestIdentityConfiguration(integrationTestAssistant, identityOptions);

        integrationTestAssistant.AddLexicomEntityFrameworkIdentity<TDbContext, TUser, TRole, TKey>();

        PostTestIdentityConfiguration(integrationTestAssistant);
    }
    /// <exception cref="ArgumentNullException"/>
    public static void AddLexicomEntityFrameworkIdentityForTesting<TDbContext, TUser, TRole, TKey, TUserRole>(this IntegrationTestAssistant integrationTestAssistant, IdentityOptions? identityOptions = null)
        where TDbContext : IdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, TUserRole, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
    {
        ArgumentNullException.ThrowIfNull(integrationTestAssistant);

        PreTestIdentityConfiguration(integrationTestAssistant, identityOptions);

        integrationTestAssistant.AddLexicomEntityFrameworkIdentity<TDbContext, TUser, TRole, TKey, TUserRole>();

        PostTestIdentityConfiguration(integrationTestAssistant);
    }
    /// <exception cref="ArgumentNullException"/>
    public static void AddLexicomEntityFrameworkIdentityForTesting<TDbContext, TUser, TRole, TKey, TUserRole, TUserClaim, TUserLogin, TRoleClaim, TUserToken>(this IntegrationTestAssistant integrationTestAssistant, IdentityOptions? identityOptions = null)
        where TDbContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
    {
        ArgumentNullException.ThrowIfNull(integrationTestAssistant);

        PreTestIdentityConfiguration(integrationTestAssistant, identityOptions);

        integrationTestAssistant.AddLexicomEntityFrameworkIdentity<TDbContext, TUser, TRole, TKey, TUserRole, TUserClaim, TUserLogin, TRoleClaim, TUserToken>();

        PostTestIdentityConfiguration(integrationTestAssistant);
    }

    private static void PreTestIdentityConfiguration(IntegrationTestAssistant integrationTestAssistant, IdentityOptions? identityOptions)
    {
        if (identityOptions is not null)
        {
            integrationTestAssistant.Configuration.AddInMemoryCollection(identityOptions);
        }
    }

    private static void PostTestIdentityConfiguration(IntegrationTestAssistant integrationTestAssistant)
    {
        integrationTestAssistant.TryAddSingleton<IHttpContextAccessor>(sp =>
        {
            return new MockDefaultHttpContextAccessor(sp);
        });

        integrationTestAssistant.AddAuthentication();
    }
}
