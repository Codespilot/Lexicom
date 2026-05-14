using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Reflection;

namespace Lexicom.Mvvm.Extensions;

public static class MessengerExtensions
{
    private static MethodInfo RegisterMethodInfo => field ??= typeof(MessengerExtensions).GetMethod(nameof(AsyncRegister), BindingFlags.Public | BindingFlags.Static) ?? throw new UnreachableException($"The method '{nameof(AsyncRegister)}' was not found."));

    /// <exception cref="ArgumentNullException"></exception>
    public static void AsyncRegister<TMessage>(this IMessenger messenger, IAsyncRecipient<TMessage> recipient) where TMessage : AsyncMessage
    {
        ArgumentNullException.ThrowIfNull(messenger);
        ArgumentNullException.ThrowIfNull(recipient);

        if (messenger is AsyncMessenger asyncMessenger)
        {
            asyncMessenger.AsyncRegister(recipient);
        }
        else
        {
            throw new NotSupportedException($"The registered '{nameof(IMessenger)}' ('{messenger?.GetType()?.Name ?? "null"}') must be of the type or derived from the type '{nameof(AsyncMessenger)}' in order to use async functions.");
        }
    }

    /// <exception cref="ArgumentNullException"></exception>
    public static void AsyncRegisterAll(this IMessenger messenger, object obj)
    {
        ArgumentNullException.ThrowIfNull(messenger);
        ArgumentNullException.ThrowIfNull(obj);

        IEnumerable<Type> asyncRecipientTypes = obj
            .GetType()
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRecipient<>));

        foreach (Type asyncRecipientType in asyncRecipientTypes)
        {
            Type? messageType = asyncRecipientType
                .GetGenericArguments()
                .FirstOrDefault();

            if (messageType is not null)
            {
                MethodInfo registerMethod = RegisterMethodInfo.MakeGenericMethod(messageType);

                registerMethod.Invoke(null,
                [
                    messenger,
                    obj,
                ]);
            }
        }
    }
}
