using Lexicom.UnitTesting;
using Lexicom.Validation.Amenities.Extensions;
using Lexicom.Validation.Amenities.UnitTests.ModelsForTests.RuleSets;
using Lexicom.Validation.Extensions;

namespace Lexicom.Validation.Amenities.UnitTests.PropertyValidators;

public class GreaterThanUnitTests
{
    [Fact]
    public async Task Message_Is_Expected()
    {
        var uta = new UnitTestAttendant();

        uta.AddLexicomValidation(options =>
        {
            options.AddAmenities();
            options.AddRuleSets<AssemblyScanMarker>();
            options.AddValidators<AssemblyScanMarker>();
        });

        var validator = uta.Get<IRuleSetValidator<GreaterThanRuleSet, string?>>();

        await validator.ValidateAsync("4", TestContext.Current.CancellationToken);

        Assert.Equal("Must be greater than 5.", validator.ValidationErrors.First());

        validator.Validate("4");

        Assert.Equal("Must be greater than 5.", validator.ValidationErrors.First());
    }
}
