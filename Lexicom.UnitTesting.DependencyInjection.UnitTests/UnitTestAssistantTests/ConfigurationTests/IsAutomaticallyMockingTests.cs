using Lexicom.UnitTesting.DependencyInjection.Exceptions;
using Lexicom.UnitTesting.DependencyInjection.Extensions;
using Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;
using NSubstitute;

namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.UnitTestAssistantTests.ConfigurationTests;

public class IsAutomaticallyMockingTests
{
    [Fact]
    public void Successfully_Make_When_Not_Automatically_Mocking_But_Dependency_Is_Mocked()
    {
        //arrange
        using var ta = new UnitTestAssistant(new UnitTestAssistantConfiguration
        {
            IsAutomaticallyMocking = false,
        });

        ta.Mock<IServiceDependencyIntReturnMethod>();

        //act
        var uot = ta.Make<ServiceWithDependency>();

        //assert
        Assert.NotNull(uot);
        Assert.NotNull(uot._serviceDependencyIntReturnMethod);

        Assert.False(uot.IsSubstitute());
        Assert.True(uot._serviceDependencyIntReturnMethod.IsSubstitute());
    }

    [Fact]
    public void Successfully_Make_When_Not_Automatically_Mocking_But_All_Dependency_Is_Manually_Provided()
    {
        //arrange
        using var ta = new UnitTestAssistant(new UnitTestAssistantConfiguration
        {
            IsAutomaticallyMocking = false,
        });

        var mockableService = new ServiceDependencyIntReturnMethod();

        //act
        var uot = ta.Make<ServiceWithDependency>(mockableService);

        //assert
        Assert.NotNull(uot);
        Assert.NotNull(uot._serviceDependencyIntReturnMethod);

        Assert.False(uot.IsSubstitute());
        Assert.False(uot._serviceDependencyIntReturnMethod.IsSubstitute());
    }

    [Fact]
    public void Successfully_Make_When_Not_Automatically_Mocking_But_All_Dependencies_Are_Mocked_Or_Manually_Provided()
    {
        //arrange
        using var ta = new UnitTestAssistant(new UnitTestAssistantConfiguration
        {
            IsAutomaticallyMocking = false,
        });

        ta.Mock<IServiceDependencyVoidReturnMethod>();
        ta.Mock<IServiceDependencyIntReturnMethod>();

        var mockServiceDependencyStringReturnMethod = Substitute.For<IServiceDependencyStringReturnMethod>();
        var mockServiceDependencyReferenceTypeReturnMethod = Substitute.For<IServiceDependencyReferenceTypeReturnMethod>();

        //act
        var uot = ta.Make<ServiceWithInterfaceDependencies>(mockServiceDependencyStringReturnMethod, mockServiceDependencyReferenceTypeReturnMethod);

        //assert
        Assert.NotNull(uot);
        Assert.NotNull(uot._serviceDependencyVoidReturnMethod);
        Assert.NotNull(uot._serviceDependencyIntReturnMethod);
        Assert.NotNull(uot._serviceDependencyStringReturnMethod);
        Assert.NotNull(uot._serviceDependencyReferenceTypeReturnMethod);

        Assert.False(uot.IsSubstitute());
        Assert.True(uot._serviceDependencyVoidReturnMethod.IsSubstitute());
        Assert.True(uot._serviceDependencyIntReturnMethod.IsSubstitute());
        Assert.True(uot._serviceDependencyStringReturnMethod.IsSubstitute());
        Assert.True(uot._serviceDependencyReferenceTypeReturnMethod.IsSubstitute());
    }

    [Fact]
    public void Fail_Make_When_Not_Automatically_Mocking_And_Dependency_Is_Not_Mocked_Or_Manually_Provided()
    {
        //arrange
        using var ta = new UnitTestAssistant(new UnitTestAssistantConfiguration
        {
            IsAutomaticallyMocking = false,
        });

        //assert
        Assert.Throws<PullNotMockedException>(() =>
        {
            //act
            var uot = ta.Make<ServiceWithDependency>();
        });
    }
}
