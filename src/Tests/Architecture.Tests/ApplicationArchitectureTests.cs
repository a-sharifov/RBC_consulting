using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using WebApp.Application.Common.CQRS;

namespace Architecture.Tests;

public sealed class ApplicationArchitectureTests
{
    private static readonly Assembly ApplicationAssembly = WebApp.Application.AssemblyReference.Assembly;

    [Fact]
    public void Commands_ShouldHaveNameEndingWithCommand()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("Commands should have names ending with 'Command'");
    }

    [Fact]
    public void Queries_ShouldHaveNameEndingWithQuery()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("Queries should have names ending with 'Query'");
    }

    [Fact]
    public void Handlers_ShouldHaveNameEndingWithHandler()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Or()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("Handlers should have names ending with 'Handler'");
    }

    [Fact]
    public void Commands_ShouldResideInCommandsNamespace()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .ResideInNamespaceContaining("Commands")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("Commands should reside in the 'Commands' namespace");
    }

    [Fact]
    public void CommandHandlers_ShouldResideInCommandsNamespace()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .ResideInNamespaceContaining("Commands")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("Command handlers should reside in the 'Commands' namespace");
    }

    [Fact]
    public void QueryHandlers_ShouldResideInQueriesNamespace()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .ResideInNamespaceContaining("Queries")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("Query handlers should reside in the 'Queries' namespace");
    }

}
