using Lexicom.Testing.DependencyInjection.EntityFramework.UnitTests.Constructs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lexicom.Testing.DependencyInjection.EntityFramework.UnitTests.Constructs.Databases;

public class PeopleDbContext : DbContext
{
    public PeopleDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Person> People { get; set; }
    public DbSet<Home> Homes { get; set; }
}
