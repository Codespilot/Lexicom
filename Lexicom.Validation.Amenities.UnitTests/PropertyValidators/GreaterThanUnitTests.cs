using Lexicom.UnitTesting.DependencyInjection;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Amenities.UnitTests.Constructs.RuleSets;
using Lexicom.Validation.Extensions;

namespace Lexicom.Validation.Amenities.UnitTests.PropertyValidators;

public class GreaterThanUnitTests
{
    [Fact]
    public async Task Message_Is_Expected()
    {
        var ita = new IntegrationTestAssistant();

        ita.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = ita.Make<IRuleSetValidator<GreaterThanRuleSet, string?>>();

        await validator.ValidateAsync("4", TestContext.Current.CancellationToken);

        Assert.Equal("Must be greater than 5.", validator.ValidationErrors.First());

        validator.Validate("4");

        Assert.Equal("Must be greater than 5.", validator.ValidationErrors.First());
    }
}
