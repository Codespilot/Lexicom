using Microsoft.EntityFrameworkCore;

namespace Lexicom.UnitTesting.DependencyInjection;

public class UnitTestAssistant<TDbContext> : UnitTestAssistant where TDbContext : DbContext
{
    public UnitTestAssistant() : this(new UnitTestAssistantConfiguration())
    {
    }
    public UnitTestAssistant(UnitTestAssistantConfiguration configuration) : base(configuration)
    {
        Database = this.Database<TDbContext>();
    }

    public TDbContext Database { get; }
}
