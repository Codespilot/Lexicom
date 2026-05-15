namespace Lexicom.UnitTesting.DependencyInjection.Exceptions;

public class MakeTypeIsNoRegisteredException(Type? type) : Exception($"Make cannot resolve the type '{type?.Name ?? "null"}', this is likly because that type is an interface and you havent registered the implementation type in the '{nameof(IntegrationTestAssistant)}' service collection.")
{
}
