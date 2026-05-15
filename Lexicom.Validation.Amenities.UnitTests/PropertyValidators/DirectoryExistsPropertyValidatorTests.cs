using FluentValidation;
using Lexicom.UnitTesting.DependencyInjection;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Amenities.PropertyValidators;
using Lexicom.Validation.Extensions;

namespace Lexicom.Validation.Amenities.UnitTests.PropertyValidators;

public class DirectoryExistsPropertyValidatorTests
{
    [Fact]
    public Task Has_Error_Message()
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = new DirectoryExistsPropertyValidator<string?>();

        bool result = validator.IsValid(new ValidationContext<string?>(""), "tests");

        //await validator.ValidateAsync("abc");

        //Assert.True(validator.err)

        //validator.ValidationErrors.Should().HaveCount(1);
        //validator.ValidationErrors.First().Should().Be("Must contain only digits.");

        return Task.CompletedTask;
    }
}
