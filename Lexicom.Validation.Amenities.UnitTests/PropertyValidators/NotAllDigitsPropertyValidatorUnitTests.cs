using Lexicom.UnitTesting;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Amenities.UnitTests.ModelsForTests.RuleSets;
using Lexicom.Validation.Extensions;

namespace Lexicom.Validation.Amenities.UnitTests.PropertyValidators;

public class NotAllDigitsPropertyValidatorUnitTests
{
    [Theory]
    [InlineData("1")]
    [InlineData("123")]
    public async Task Message_Is_Expected(string input)
    {
        var uta = new UnitTestAttendant();

        uta.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = uta.Get<IRuleSetValidator<NotAllDigitsRuleSet, string?>>();

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
        var uta = new UnitTestAttendant();

        uta.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = uta.Get<IRuleSetValidator<NotAllDigitsRuleSet, string?>>();

        await validator.ValidateAsync(input, TestContext.Current.CancellationToken);

        Assert.True(validator.IsValid);
        Assert.Empty(validator.ValidationErrors);

        validator.Validate(input);

        Assert.True(validator.IsValid);
        Assert.Empty(validator.ValidationErrors);
    }
}