using System.Reflection;
using NetArchTest.Rules;

namespace Personix.ArchitectureTests;

/// <summary>
/// Standard Clean Architecture layer dependency rules built on <c>NetArchTest.Rules</c>.
/// </summary>
/// <remarks>
/// Usage in your Architecture.Tests project:
/// <code>
/// var assemblies = new MyLayerAssemblies();
/// foreach (var result in CleanArchitectureRules.VerifyAll(assemblies))
///     Assert.True(result.IsSuccessful, result.FailingMessage());
/// </code>
/// </remarks>
public static class CleanArchitectureRules
{
    // -------------------------------------------------------------------------
    // Aggregate helper
    // -------------------------------------------------------------------------

    /// <summary>
    /// Runs all applicable standard rules for the provided assembly map.
    /// Rules are skipped when the corresponding assembly is <see langword="null"/>.
    /// </summary>
    public static IEnumerable<ArchitectureTestResult> VerifyAll(ICleanArchitectureAssemblies assemblies)
    {
        yield return DomainMustNotDependOnOtherLayers(assemblies);

        yield return ApplicationMustNotDependOnInfrastructureOrPresentation(assemblies);

        if (assemblies.Infrastructure is not null)
            yield return InfrastructureMustNotDependOnPresentation(assemblies);

        if (assemblies.Persistence is not null)
        {
            yield return PersistenceMustNotDependOnInfrastructure(assemblies);
            yield return PersistenceMustNotDependOnPresentation(assemblies);
        }
    }

    // -------------------------------------------------------------------------
    // Individual rules
    // -------------------------------------------------------------------------

    /// <summary>Domain must not depend on any other application layer.</summary>
    public static ArchitectureTestResult DomainMustNotDependOnOtherLayers(
        ICleanArchitectureAssemblies assemblies)
    {
        var forbidden = NonNull(
            assemblies.Application,
            assemblies.Infrastructure,
            assemblies.Persistence,
            assemblies.Presentation);

        var result = Types
            .InAssembly(assemblies.Domain)
            .Should()
            .NotHaveDependencyOnAny(NamespaceRoots(forbidden))
            .GetResult();

        return Wrap(nameof(DomainMustNotDependOnOtherLayers), result);
    }

    /// <summary>Application must not depend on Infrastructure, Persistence or Presentation.</summary>
    public static ArchitectureTestResult ApplicationMustNotDependOnInfrastructureOrPresentation(
        ICleanArchitectureAssemblies assemblies)
    {
        var forbidden = NonNull(
            assemblies.Infrastructure,
            assemblies.Persistence,
            assemblies.Presentation);

        if (forbidden.Length == 0)
            return ArchitectureTestResult.Passed(nameof(ApplicationMustNotDependOnInfrastructureOrPresentation));

        var result = Types
            .InAssembly(assemblies.Application)
            .Should()
            .NotHaveDependencyOnAny(NamespaceRoots(forbidden))
            .GetResult();

        return Wrap(nameof(ApplicationMustNotDependOnInfrastructureOrPresentation), result);
    }

    /// <summary>Infrastructure must not depend on Presentation.</summary>
    public static ArchitectureTestResult InfrastructureMustNotDependOnPresentation(
        ICleanArchitectureAssemblies assemblies)
    {
        if (assemblies.Infrastructure is null || assemblies.Presentation is null)
            return ArchitectureTestResult.Passed(nameof(InfrastructureMustNotDependOnPresentation));

        var result = Types
            .InAssembly(assemblies.Infrastructure)
            .Should()
            .NotHaveDependencyOn(assemblies.Presentation.GetName().Name!)
            .GetResult();

        return Wrap(nameof(InfrastructureMustNotDependOnPresentation), result);
    }

    /// <summary>Persistence must not depend on Infrastructure.</summary>
    public static ArchitectureTestResult PersistenceMustNotDependOnInfrastructure(
        ICleanArchitectureAssemblies assemblies)
    {
        if (assemblies.Persistence is null || assemblies.Infrastructure is null)
            return ArchitectureTestResult.Passed(nameof(PersistenceMustNotDependOnInfrastructure));

        var result = Types
            .InAssembly(assemblies.Persistence)
            .Should()
            .NotHaveDependencyOn(assemblies.Infrastructure.GetName().Name!)
            .GetResult();

        return Wrap(nameof(PersistenceMustNotDependOnInfrastructure), result);
    }

    /// <summary>Persistence must not depend on Presentation.</summary>
    public static ArchitectureTestResult PersistenceMustNotDependOnPresentation(
        ICleanArchitectureAssemblies assemblies)
    {
        if (assemblies.Persistence is null || assemblies.Presentation is null)
            return ArchitectureTestResult.Passed(nameof(PersistenceMustNotDependOnPresentation));

        var result = Types
            .InAssembly(assemblies.Persistence)
            .Should()
            .NotHaveDependencyOn(assemblies.Presentation.GetName().Name!)
            .GetResult();

        return Wrap(nameof(PersistenceMustNotDependOnPresentation), result);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static Assembly[] NonNull(params Assembly?[] assemblies) =>
        assemblies.Where(a => a is not null).Select(a => a!).ToArray();

    private static string[] NamespaceRoots(Assembly[] assemblies) =>
        assemblies.Select(a => a.GetName().Name!).ToArray();

    private static ArchitectureTestResult Wrap(string ruleName, TestResult result) =>
        new(ruleName, result.IsSuccessful, result.FailingTypeNames ?? []);
}
