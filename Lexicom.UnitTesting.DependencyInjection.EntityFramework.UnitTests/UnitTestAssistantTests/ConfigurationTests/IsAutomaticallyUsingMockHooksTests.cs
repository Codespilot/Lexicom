using Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.Constructs.Databases;
using Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.Constructs.Services;
using Lexicom.UnitTesting.DependencyInjection.Exceptions;
using Lexicom.UnitTesting.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.UnitTestAssistantTests.ConfigurationTests;

public class IsAutomaticallyUsingMockHooksTests
{
    [Fact]
    public void Successfully_Make_When_Not_Automatically_Mocking_Using_Hooks_But_DbContextFactory_Is_Mocked_Using_Databaes()
    {
        //arrange
        using var ta = new UnitTestAssistant(new UnitTestAssistantConfiguration
        {
            IsAutomaticallyUsingMockHooks = false,
        });

        ta.Database<PeopleDbContext>();

        //act
        var uot = ta.Make<PeopleService>();

        //assert
        Assert.NotNull(uot);
        Assert.NotNull(uot._dbContextFactory);

        Assert.False(uot.IsSubstitute());
        Assert.False(uot._dbContextFactory.IsSubstitute());
        Assert.True(uot._dbContextFactory is SqliteInMemoryTestDbContextFactory<PeopleDbContext>);
    }

    [Fact]
    public void Successfully_Make_When_Not_Automatically_Mocking_Using_Hooks_But_DbContextFactory_Is_Mocked_Using_Mock()
    {
        //arrange
        using var ta = new UnitTestAssistant(new UnitTestAssistantConfiguration
        {
            IsAutomaticallyUsingMockHooks = false,
        });

        ta.Mock<IDbContextFactory<PeopleDbContext>>();

        //act
        var uot = ta.Make<PeopleService>();

        //assert
        Assert.NotNull(uot);
        Assert.NotNull(uot._dbContextFactory);

        Assert.False(uot.IsSubstitute());
        Assert.True(uot._dbContextFactory.IsSubstitute());
    }

    [Fact]
    public void Successfully_Make_When_Not_Automatically_Mocking_Using_Hooks_And_DbContextFactory_Is_Not_Mocked_Or_Manually_Provided()
    {
        //arrange
        using var ta = new UnitTestAssistant(new UnitTestAssistantConfiguration
        {
            IsAutomaticallyUsingMockHooks = false,
        });

        //act
        var uot = ta.Make<PeopleService>();

        //assert
        Assert.NotNull(uot);
        Assert.NotNull(uot._dbContextFactory);

        Assert.False(uot.IsSubstitute());
        Assert.True(uot._dbContextFactory.IsSubstitute());
    }
}
