using Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.Constructs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.Constructs.Databases;

public class ColorsDbContext : DbContext
{
    public ColorsDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Color> Colors { get; set; }
}
