using Lexicom.UnitTesting.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.IntegrationTestAssistantTests;

public class TempTests
{
    [Fact]
    public void Test1()
    {
        var ita = new IntegrationTestAssistant();

        ita.AddSingleton<IOtherDependency, OtherDependency>();

        var model = new MyModel
        {
            Id = 1,
        };

        var uot = ita.Make<MyService>(model);

        int result = uot.DoWork();

        Assert.False(uot._otherDependency.IsSubstitute());
        Assert.True(uot._subDependency.IsSubstitute());

        Assert.Equal(6, result);
    }

    [Fact]
    public void Test2()
    {
        var ita = new IntegrationTestAssistant();

        ita.AddSingleton<IOtherDependency, OtherDependency>();
        ita.AddSingleton<IMyService, MyService>();

        var model = new MyModel
        {
            Id = 1,
        };

        var uot = ita.Make<IMyService>(model);

        int result = uot.DoWork();

        Assert.Equal(6, result);
    }

    [Fact]
    public void Test3()
    {
        var ita = new IntegrationTestAssistant();

        var uot = ita.Make<OtherDependency>();

        int result = uot.GetNum();

        Assert.Equal(3, result);
    }

    [Fact]
    public void Test4()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        //assert
        Assert.Throws<Exception>(() =>
        {
            //act
            var uot = ita.Make<IOtherDependency>();
        });
    }

    public class MyModel
    {
        public required int Id { get; init; }
    }

    public interface IMyService
    {
        int DoWork();
    }
    private class MyService : IMyService
    {
        public readonly IOtherDependency _otherDependency;
        public readonly ISubDependency _subDependency;

        public MyService(
            MyModel model,
            IOtherDependency otherDependency, 
            ISubDependency subDependency)
        {
            _otherDependency = otherDependency;
            _subDependency = subDependency;

            Model = model;
        }

        public MyModel Model { get; }

        public int DoWork()
        {
            return _subDependency.Zero() + Model.Id + 2 + _otherDependency.GetNum();
        }
    }

    public interface IOtherDependency
    {
        int GetNum();
    }
    public class OtherDependency : IOtherDependency
    {
        public int GetNum()
        {
            return 3;
        }
    }
    public interface ISubDependency
    {
        int Zero();
    }
}
