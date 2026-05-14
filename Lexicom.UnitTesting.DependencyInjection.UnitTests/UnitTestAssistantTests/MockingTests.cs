using Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;
using NSubstitute;

namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.UnitTestAssistantTests;

public class MockingTests
{
    [Fact]
    public async Task Mock_So_Returns_Specific_Value()
    {
        //arrange
        using var ta = new UnitTestAssistant();

        int expectedNumber = 123;
        int asyncExpectedNumber = 321;
        ta.Mock<IServiceDependencyIntReturnMethod>().So(ms =>
        {
            ms.GetValueTypeIntMethod().Returns(expectedNumber);
            ms.GetValueTypeIntMethodAsync().Returns(asyncExpectedNumber);
        });

        //act
        var uot = ta.Make<ServiceWithDependency>();

        int number = uot._serviceDependencyIntReturnMethod.GetValueTypeIntMethod();
        int asyncNumber = await uot._serviceDependencyIntReturnMethod.GetValueTypeIntMethodAsync();

        //assert
        Assert.Equal(expectedNumber, number);
        Assert.Equal(asyncExpectedNumber, asyncNumber);
    }
}
