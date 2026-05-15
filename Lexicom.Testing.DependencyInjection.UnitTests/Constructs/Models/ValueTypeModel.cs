namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Models;

public struct ValueTypeModel
{
    public required Guid Id { get; init; }
    public string? Value { get; set; }
}
