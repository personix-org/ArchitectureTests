using System.Reflection;
using NetArchTest.Rules;

namespace Common.ArchitectureTests;

/// <summary>
/// Maps the assemblies of a solution to Clean Architecture layers.
/// Implement this in your Architecture.Tests project and pass it to
/// <see cref="CleanArchitectureRules.VerifyAll"/>.
/// </summary>
public interface ICleanArchitectureAssemblies
{
    /// <summary>Core domain entities and value objects. No outward dependencies allowed.</summary>
    Assembly Domain { get; }

    /// <summary>Use cases / application services. May depend on Domain only.</summary>
    Assembly Application { get; }

    /// <summary>External adapters (DB, git, HTTP clients …). Must not be referenced by Application.</summary>
    Assembly? Infrastructure { get; }

    /// <summary>Persistence layer. Must not be referenced by Application or Infrastructure.</summary>
    Assembly? Persistence { get; }

    /// <summary>UI / entry-point layer. May depend on Application but not vice-versa.</summary>
    Assembly? Presentation { get; }
}
