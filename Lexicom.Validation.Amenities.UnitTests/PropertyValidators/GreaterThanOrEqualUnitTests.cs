using Lexicom.UnitTesting.DependencyInjection;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Amenities.UnitTests.Constructs.RuleSets;
using Lexicom.Validation.Extensions;

namespace Lexicom.Validation.Amenities.UnitTests.PropertyValidators;

public class GreaterThanOrEqualUnitTests
{
    [Fact]
    public async Task Has_Error_Message()
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = ita.Make<IRuleSetValidator<NumberStringRuleSet, string?>>();

        await validator.ValidateAsync("abc", TestContext.Current.CancellationToken);

        Assert.Single(validator.ValidationErrors);
        Assert.Equal("Must contain only digits.", validator.ValidationErrors.First());
    }

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    public async Task Has_Error_Message_Greater_Than(string value)
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = ita.Make<IRuleSetValidator<NumberStringRuleSet, string?>>();

        await validator.ValidateAsync(value, TestContext.Current.CancellationToken);

        Assert.Single(validator.ValidationErrors);
        Assert.Equal("Must be greater than or equal to 3.", validator.ValidationErrors.First());
    }

    [Theory]
    [InlineData("3")]
    [InlineData("4")]
    [InlineData("1000")]
    public async Task Has_No_Error_Message(string value)
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = ita.Make<IRuleSetValidator<NumberStringRuleSet, string?>>();

        await validator.ValidateAsync(value, TestContext.Current.CancellationToken);

        Assert.Empty(validator.ValidationErrors);
    }
}
