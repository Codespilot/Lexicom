using Lexicom.UnitTesting.DependencyInjection.Exceptions;
using Lexicom.UnitTesting.DependencyInjection.Extensions;
using Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Models;
using Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.IntegrationTestAssistantTests;

public class MakeTests
{
    [Fact]
    public async Task Successfully_Make_Type_With_Mixed_Dependencies()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        ita.AddSingleton<IServiceDependencyIntReturnMethod, ServiceDependencyIntReturnMethod>();

        int intValueType = 123;
        var referenceTypeModel = new ReferenceTypeModel();
        var valueTypeModel = new ValueTypeModel
        {
            Id = Guid.NewGuid(),
        };

        //act
        var uot = ita.Make<ServiceWithMixedDependencies>(intValueType, referenceTypeModel, valueTypeModel);

        //assert
        Assert.NotNull(uot);
        Assert.NotNull(uot._serviceDependencyVoidReturnMethod);
        Assert.NotNull(uot._serviceDependencyIntReturnMethod);
        Assert.NotNull(uot._serviceDependencyStringReturnMethod);
        Assert.NotNull(uot._serviceDependencyReferenceTypeReturnMethod);

        Assert.False(uot.IsSubstitute());
        Assert.False(uot.ReferenceTypeModel.IsSubstitute());
        Assert.False(uot._serviceDependencyIntReturnMethod.IsSubstitute());

        Assert.True(uot._serviceDependencyVoidReturnMethod.IsSubstitute());
        Assert.True(uot._serviceDependencyStringReturnMethod.IsSubstitute());
        Assert.True(uot._serviceDependencyReferenceTypeReturnMethod.IsSubstitute());

        int mockableServiceNumber = uot._serviceDependencyIntReturnMethod.GetValueTypeIntMethod();
        int asyncMockableServiceNumber = await uot._serviceDependencyIntReturnMethod.GetValueTypeIntMethodAsync();

        Assert.Equal(ServiceDependencyIntReturnMethod.NUMBER, mockableServiceNumber);
        Assert.Equal(ServiceDependencyIntReturnMethod.NUMBER, asyncMockableServiceNumber);
        Assert.Equal(intValueType, uot.IntValueType);
        Assert.Equal(referenceTypeModel, uot.ReferenceTypeModel);
        Assert.Equal(valueTypeModel, uot.ValueTypeModel);
    }

    [Fact]
    public void Fail_To_Make_When_Type_Is_Not_Registered()
    {
        //arrange
        var ita = new IntegrationTestAssistant();

        //assert
        Assert.Throws<MakeTypeIsNoRegisteredException>(() =>
        {
            //act
            var uot = ita.Make<IServiceDependencyVoidReturnMethod>();
        });
    }
}
