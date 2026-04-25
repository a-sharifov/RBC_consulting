using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public sealed class CrossLayerDependencyTests
{
    private static readonly Assembly DomainAssembly = WebApp.Domain.AssemblyReference.Assembly;

    [Theory]
    [InlineData("WebApp.Infrastructure")]
    [InlineData("WebApp.Contracts")]
    [InlineData("WebApp.Application")]
    [InlineData("WebApp")]
    public void Domain_ShouldNotDependOn(string layer)
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();

        result.IsSuccessful.Should().BeTrue($"Domain layer should not depend on {layer} layer");
    }

    [Theory]
    [InlineData("WebApp.Application")]
    [InlineData("WebApp")]
    public void Infrastructure_ShouldDependOn(string layer)
    {
        var result = Types.InAssembly(WebApp.Infrastructure.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();

        result.IsSuccessful.Should().BeTrue($"Infrastructure layer should not depend on {layer} layer");
    }

    [Theory]
    [InlineData("WebApp.Infrastructure")]
    public void Application_ShouldDependOn(string layer)
    {
        var result = Types.InAssembly(WebApp.Application.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();
        result.IsSuccessful.Should().BeTrue($"Application layer should not depend on {layer} layer");
    }

    [Theory]
    [InlineData("WebApp.Application")]
    [InlineData("WebApp.Infrastructure")]
    public void Contracts_ShouldNotDependOn(string layer)
    {
        var result = Types.InAssembly(WebApp.Contracts.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOn(layer)
            .GetResult();
        result.IsSuccessful.Should().BeTrue($"Contracts layer should not depend on {layer} layer");
    }
}
