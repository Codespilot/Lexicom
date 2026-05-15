using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexicom.Testing.DependencyInjection.EntityFramework.UnitTests.Constructs.Entities;

public class Color : IEntityTypeConfiguration<Color>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }

    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder
            .ToTable("Colors")
            .HasKey(c => c.Id);
    }
}
