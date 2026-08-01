using Xunit;

namespace Common.ArchitectureTests;

/// <summary>
/// Base class for xUnit architecture test classes.
/// Derive from this, implement <see cref="Assemblies"/> and all standard
/// Clean Architecture rules are automatically verified as individual facts.
/// </summary>
public abstract class CleanArchitectureTestBase
{
    /// <summary>Assembly map for the solution under test.</summary>
    protected abstract ICleanArchitectureAssemblies Assemblies { get; }

    [Fact]
    public void Domain_MustNotDependOn_OtherLayers()
    {
        var result = CleanArchitectureRules.DomainMustNotDependOnOtherLayers(Assemblies);
        Assert.True(result.IsSuccessful, result.FailingMessage());
    }

    [Fact]
    public void Application_MustNotDependOn_InfrastructureOrPresentation()
    {
        var result = CleanArchitectureRules.ApplicationMustNotDependOnInfrastructureOrPresentation(Assemblies);
        Assert.True(result.IsSuccessful, result.FailingMessage());
    }

    [Fact]
    public void Infrastructure_MustNotDependOn_Presentation()
    {
        var result = CleanArchitectureRules.InfrastructureMustNotDependOnPresentation(Assemblies);
        Assert.True(result.IsSuccessful, result.FailingMessage());
    }

    [Fact]
    public void Persistence_MustNotDependOn_Infrastructure()
    {
        var result = CleanArchitectureRules.PersistenceMustNotDependOnInfrastructure(Assemblies);
        Assert.True(result.IsSuccessful, result.FailingMessage());
    }

    [Fact]
    public void Persistence_MustNotDependOn_Presentation()
    {
        var result = CleanArchitectureRules.PersistenceMustNotDependOnPresentation(Assemblies);
        Assert.True(result.IsSuccessful, result.FailingMessage());
    }
}
