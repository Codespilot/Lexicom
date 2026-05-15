using NSubstitute.Core;
using NSubstitute.Exceptions;

namespace Lexicom.Testing.DependencyInjection.Extensions;

public static class ObjectExtensions
{
    public static bool IsSubstitute(this object? instance)
    {
        if (instance is null)
        {
            return false;
        }

        try
        {
            SubstitutionContext.Current.GetCallRouterFor(instance);

            return true;
        }
        catch (NotASubstituteException)
        {
            return false;
        }
    }
}
