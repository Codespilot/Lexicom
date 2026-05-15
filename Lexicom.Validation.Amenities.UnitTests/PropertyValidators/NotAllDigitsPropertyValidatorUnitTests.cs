using Lexicom.UnitTesting.DependencyInjection;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Amenities.UnitTests.Constructs.RuleSets;
using Lexicom.Validation.Extensions;

namespace Lexicom.Validation.Amenities.UnitTests.PropertyValidators;

public class NotAllDigitsPropertyValidatorUnitTests
{
    [Theory]
    [InlineData("1")]
    [InlineData("123")]
    public async Task Message_Is_Expected(string input)
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = ita.Make<IRuleSetValidator<NotAllDigitsRuleSet, string?>>();

        await validator.ValidateAsync(input, TestContext.Current.CancellationToken);

        Assert.False(validator.IsValid);
        Assert.Equal("Must not contain only digits.", validator.ValidationErrors.First());

        validator.Validate(input);

        Assert.False(validator.IsValid);
        Assert.Equal("Must not contain only digits.", validator.ValidationErrors.First());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a")]
    [InlineData("abc")]
    [InlineData("a1")]
    [InlineData("abc123")]
    [InlineData("1 2 3")]
    public async Task Message_Is_Not_Expected(string? input)
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = ita.Make<IRuleSetValidator<NotAllDigitsRuleSet, string?>>();

        await validator.ValidateAsync(input, TestContext.Current.CancellationToken);

        Assert.True(validator.IsValid);
        Assert.Empty(validator.ValidationErrors);

        validator.Validate(input);

        Assert.True(validator.IsValid);
        Assert.Empty(validator.ValidationErrors);
    }
}