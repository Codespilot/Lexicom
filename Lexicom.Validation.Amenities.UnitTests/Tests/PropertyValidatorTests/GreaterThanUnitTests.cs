using Lexicom.Testing.DependencyInjection;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Amenities.UnitTests.Constructs;
using Lexicom.Validation.Amenities.UnitTests.Constructs.RuleSets;
using Lexicom.Validation.Extensions;

namespace Lexicom.Validation.Amenities.UnitTests.Tests.PropertyValidatorTests;

public class GreaterThanUnitTests
{
    [Fact]
    public async Task Message_Is_Expected()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        //act
        var validator = ita.Make<IRuleSetValidator<GreaterThanRuleSet, string?>>();

        await validator.ValidateAsync("4", TestContext.Current.CancellationToken);
        string asyncMessage = validator.ValidationErrors.First();

        validator.Validate("4");
        string syncMessage = validator.ValidationErrors.First();

        //assert
        Assert.Equal("Must be greater than 5.", asyncMessage);
        Assert.Equal("Must be greater than 5.", syncMessage);
    }
}
