using System.Reflection;

namespace Lexicom.UnitTesting.DependencyInjection;

public class UnitTestAssistant : TestAssistant
{
    public UnitTestAssistant() : this(new TestAssistantConfiguration())
    {
    }
    public UnitTestAssistant(TestAssistantConfiguration configuration) : base(configuration)
    {
    }
}
