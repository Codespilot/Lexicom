using Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.Constructs.Databases;
using Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.Constructs.Entities;
using Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.Constructs.Services;
using Microsoft.EntityFrameworkCore;

namespace Lexicom.UnitTesting.DependencyInjection.EntityFramework.UnitTests.UnitTestAssistantTests;

public class DatabaseTests
{
    private static Person GetNewBob(Guid? homeId = null) => GetNewPerson("bob", 32, homeId);
    private static Person GetNewAlice(Guid? homeId = null) => GetNewPerson("Alice", 30, homeId);
    private static Person GetNewAlex(Guid? homeId = null) => GetNewPerson("Alex", 29, homeId);
    private static Person GetNewPerson(string name, int age, Guid? homeId = null)
    {
        return new Person
        {
            Id = Guid.NewGuid(),
            HomeId = homeId is null ? Guid.NewGuid() : homeId.Value,
            Name = name,
            Age = age,
            CreatedDateTimeUtc = DateTimeOffset.UtcNow,
            ModifiedDateTimeUtc = DateTimeOffset.UtcNow,
        };
    }

    [Fact]
    public async Task Successfully_Arrange_Database_For_Read_Test()
    {
        //arrange
        using var ta = new UnitTestAssistant<PeopleDbContext>();

        var bob = GetNewBob();
        var alice = GetNewAlice();
        var alex = GetNewAlex();

        ta.Database.People.Add(bob);
        ta.Database.People.Add(alice);
        ta.Database.People.Add(alex);

        await ta.Database.SaveChangesAsync();

        var uot = ta.Make<PeopleService>();

        //act
        int countOfPeopleWithAgesGreaterThanOrEqualTo30 = await uot.CountOfPeopleWithAgesGreaterThanOrEqualTo30Async();

        //assert
        Assert.Equal(2, countOfPeopleWithAgesGreaterThanOrEqualTo30);
    }

    [Fact]
    public async Task Successfully_Arrange_Database_For_Write_Test()
    {
        //arrange
        using var ta = new UnitTestAssistant<PeopleDbContext>();

        var uot = ta.Make<PeopleService>();

        //act
        await uot.AddPeople();

        //assert
        int count = await ta.Database.People.CountAsync();

        Assert.Equal(3, count);
    }

    [Fact]
    public async Task Successfully_Arrange_Database_For_Read_And_Write_Test()
    {
        //arrange
        using var ta = new UnitTestAssistant<PeopleDbContext>();

        var bob = GetNewBob();
        var alice = GetNewAlice();
        var alex = GetNewAlex();

        ta.Database.People.Add(bob);
        ta.Database.People.Add(alice);
        ta.Database.People.Add(alex);

        await ta.Database.SaveChangesAsync();

        var uot = ta.Make<PeopleService>();

        //act
        int originalCount = await uot.GetCountOfPeopleThenRemoveThemAllAsync();

        //assert
        int newCount = await ta.Database.People.CountAsync();

        Assert.Equal(3, originalCount);
        Assert.Equal(0, newCount);
    }

    [Fact]
    public async Task Successfully_Arrange_Multiple_Databases()
    {
        //arrange
        using var ta = new UnitTestAssistant();

        var colorsDb = ta.Database<ColorsDbContext>();

        var red = new Color
        {
            Id = Guid.NewGuid(),
            Name = "red",
        };
        var blue = new Color
        {
            Id = Guid.NewGuid(),
            Name = "blue",
        };
        var yellow = new Color
        {
            Id = Guid.NewGuid(),
            Name = "yellow",
        };
        var green = new Color
        {
            Id = Guid.NewGuid(),
            Name = "green",
        };

        colorsDb.Colors.Add(red);
        colorsDb.Colors.Add(blue);
        colorsDb.Colors.Add(yellow);
        colorsDb.Colors.Add(green);

        await colorsDb.SaveChangesAsync();

        var peopleDb = ta.Database<PeopleDbContext>();

        var bobAndAliceHome = new Home
        {
            Id = new Guid(),
            Address = "123 new way rd",
            CreatedDateTimeUtc = DateTimeOffset.UtcNow,
            ModifiedDateTimeUtc = DateTimeOffset.UtcNow,
        };
        var alexHome = new Home
        {
            Id = new Guid(),
            Address = "5 rd",
            CreatedDateTimeUtc = DateTimeOffset.UtcNow,
            ModifiedDateTimeUtc = DateTimeOffset.UtcNow,
        };

        peopleDb.Homes.Add(bobAndAliceHome);
        peopleDb.Homes.Add(alexHome);

        var bob = GetNewBob(bobAndAliceHome.Id);
        var alice = GetNewAlice(bobAndAliceHome.Id);
        var alex = GetNewAlex(alexHome.Id);

        peopleDb.People.Add(bob);
        peopleDb.People.Add(alice);
        peopleDb.People.Add(alex);

        await peopleDb.SaveChangesAsync();

        var uot = ta.Make<ServiceWithMultipleDbContexts>();

        //act
        await uot.RemoveEntityFirstFromDatabasesAsync();

        //assert
        int colorsCount = await colorsDb.Colors.CountAsync();
        int peopleCount = await peopleDb.People.CountAsync();
        int homesCount = await peopleDb.Homes.CountAsync();

        Assert.Equal(3, colorsCount);
        Assert.Equal(2, peopleCount);
        Assert.Equal(1, homesCount);
    }

    [Fact]
    public void Automatically_Inject_Database_Dependencies()
    {
        //arrange
        using var ta = new UnitTestAssistant();

        //act
        var uot = ta.Make<ServiceWithMultipleDbContexts>();

        //assert
        Assert.NotNull(uot._colorsDbContextFactory);
        Assert.NotNull(uot._peopleDbContextFactory);

        Assert.True(uot._colorsDbContextFactory is SqliteInMemoryTestDbContextFactory<ColorsDbContext>);
        Assert.True(uot._peopleDbContextFactory is SqliteInMemoryTestDbContextFactory<PeopleDbContext>);
    }

    //[Fact]
    //public void Fail_To_Inject_Database_Dependency_With_Too_Many_Constructors()
    //{
    //    //arrange
    //    using var ta = new TestAssistant();

    //    //assert
    //    Assert.Throws<DbContextTooManyConstructorsException>(() =>
    //    {
    //        //act
    //        var uot = ta.Make<ServiceWithDatabaseUsingMultipleConstructors>();
    //    });
    //}

    //[Fact]
    //public void Fail_To_Pull_Database_With_Too_Many_Constructors()
    //{
    //    //arrange
    //    using var ta = new TestAssistant();

    //    //assert
    //    Assert.Throws<DbContextTooManyConstructorsException>(() =>
    //    {
    //        //act
    //        var db = ta.Database<DbContextWithMultipleConstructors>();
    //    });
    //}

    //[Fact]
    //public void Fail_To_Create_TestAssistant_With_Too_Many_Constructors()
    //{
    //    //assert
    //    Assert.Throws<DbContextTooManyConstructorsException>(() =>
    //    {
    //        //arrange - act
    //        using var ta = new TestAssistant<DbContextWithMultipleConstructors>();
    //    });
    //}

    //[Fact]
    //public void Fail_To_Inject_Database_Dependency_With_Too_Many_Constructor_Parameters()
    //{
    //    //arrange
    //    using var ta = new TestAssistant();

    //    //assert
    //    Assert.Throws<DbContextTooManyConstructorParametersException>(() =>
    //    {
    //        //act
    //        var uot = ta.Make<ServiceWithDatabaseUsingMultipleConstructorParameters>();
    //    });
    //}

    //[Fact]
    //public void Fail_To_Pull_Database_With_Too_Many_Constructor_Parameters()
    //{
    //    //arrange
    //    using var ta = new TestAssistant();

    //    //assert
    //    Assert.Throws<DbContextTooManyConstructorParametersException>(() =>
    //    {
    //        //act
    //        var db = ta.Database<DbContextWithMultipleConstructorParameters>();
    //    });
    //}

    //[Fact]
    //public void Fail_To_Create_TestAssistant_With_Too_Many_Constructor_Parameters()
    //{
    //    //assert
    //    Assert.Throws<DbContextTooManyConstructorParametersException>(() =>
    //    {
    //        //arrange - act
    //        using var ta = new TestAssistant<DbContextWithMultipleConstructorParameters>();
    //    });
    //}
}
