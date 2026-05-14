using System.Diagnostics;
using System.Reflection;

namespace Lexicom.UnitTesting.DependencyInjection.Mocking;

public class MockHooksManager
{
    public delegate bool TryMockDbContextFactoryDelegate(MockManager manager, Type type);
    private static bool CheckedMockDbContextFactoryDelegateHook { get; set; }
    private static TryMockDbContextFactoryDelegate? TryMockDbContextFactoryDelegateHook { get; set; }

    public static bool TryToMockFromHooks(MockManager manager, Type type, UnitTestAssistantConfiguration unitTestAssistantConfiguration)
    {
        ArgumentNullException.ThrowIfNull(manager);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(unitTestAssistantConfiguration);

        if (unitTestAssistantConfiguration.IsAutomaticallyUsingMockHooks)
        {
            if (TryMockDbContextFactoryDelegateHook is not null)
            {
                return TryMockDbContextFactoryDelegateHook.Invoke(manager, type);
            }

            if (!CheckedMockDbContextFactoryDelegateHook)
            {
                CheckedMockDbContextFactoryDelegateHook = true;

                Type? hookType = Type.GetType("Lexicom.UnitTesting.DependencyInjection.EntityFramework.Mocking.MockHook, Lexicom.UnitTesting.DependencyInjection.EntityFramework");
                if (hookType is not null)
                {
                    const string DELEGATE_PROPERTY_NAME = "TryMockDbContextFactoryDelegate";

                    PropertyInfo? delegateProperty = hookType.GetProperty(DELEGATE_PROPERTY_NAME, BindingFlags.Public | BindingFlags.Static);
                    if (delegateProperty is null)
                    {
                        throw new UnreachableException($"The hook type '{hookType.Name}' was found but the delegate property '{DELEGATE_PROPERTY_NAME}' was not which should not be possible.");
                    }

                    object? delegateValue = delegateProperty.GetValue(null);
                    if (delegateValue is not TryMockDbContextFactoryDelegate tryMockDbContextFactoryDelegateHook)
                    {
                        throw new UnreachableException($"The hooked delegate value '{delegateValue?.GetType()?.Name ?? "null"}' was not of the type '{nameof(TryMockDbContextFactoryDelegate)}'.");
                    }

                    TryMockDbContextFactoryDelegateHook = tryMockDbContextFactoryDelegateHook;

                    return TryToMockFromHooks(manager, type, unitTestAssistantConfiguration);
                }
            }
        }

        return false;
    }
}
