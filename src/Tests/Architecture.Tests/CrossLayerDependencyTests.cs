using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public sealed class CrossLayerDependencyTests
{
    private static readonly Assembly DomainAssembly = RBC_consulting.Domain.AssemblyReference.Assembly;

    [Theory]
    [InlineData("RBC_consulting.Infrastructure")]
    [InlineData("RBC_consulting.Contracts")]
    [InlineData("RBC_consulting.Application")]
    [InlineData("RBC_consulting.Api")]

    public void Domain_ShouldNotDependOn(string layer)
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();

        result.IsSuccessful.Should().BeTrue($"Domain layer should not depend on {layer} layer");
    }

    [Theory]
    [InlineData("RBC_consulting.Application")]
    [InlineData("RBC_consulting.Api")]
    public void Infrastructure_ShouldDependOn(string layer)
    {
        var result = Types.InAssembly(RBC_consulting.Infrastructure.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();

        result.IsSuccessful.Should().BeTrue($"Infrastructure layer should not depend on {layer} layer");
    }

    [Theory]
    [InlineData("RBC_consulting.Infrastructure")]
    public void Application_ShouldDependOn(string layer)
    {
        var result = Types.InAssembly(RBC_consulting.Application.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();
        result.IsSuccessful.Should().BeTrue($"Application layer should not depend on {layer} layer");
    }

    [Theory]
    [InlineData("RBC_consulting.Application")]
    [InlineData("RBC_consulting.Infrastructure")]
    public void Contracts_ShouldNotDependOn(string layer)
    {
        var result = Types.InAssembly(RBC_consulting.Contracts.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();
        result.IsSuccessful.Should().BeTrue($"Contracts layer should not depend on {layer} layer");
    }
}
