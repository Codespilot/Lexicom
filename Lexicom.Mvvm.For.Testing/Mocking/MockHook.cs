using Lexicom.Testing.DependencyInjection;
using Lexicom.Testing.DependencyInjection.Mocking;

namespace Lexicom.Mvvm.For.Testing.Mocking;

public static class MockHook
{
    public static MockHooksManager.HookDelegate HookDelegate { get; } = TryMockDbContextFactory;

    private static bool TryMockDbContextFactory(MockManager manager, Type type)
    {
        ArgumentNullException.ThrowIfNull(manager);
        ArgumentNullException.ThrowIfNull(type);

        Type genericType = type.GetGenericTypeDefinition();
        if (genericType == typeof(IDbContextFactory<>))
        {
            Type dbContextType = type.GetGenericArguments()[0];

            Type dbContextFactoryInterfaceType = genericType.MakeGenericType(dbContextType);
            Type dbContextFactoryConcreteType = typeof(SqliteInMemoryTestDbContextFactory<>).MakeGenericType(dbContextType);

            manager.Mock(dbContextFactoryInterfaceType, MockLifetime.Singleton).With(dbContextFactoryConcreteType);

            return true;
        }

        return false;
    }
}
