using System.Reflection;
using Personix.ArchitectureTests;
using Xunit;

namespace Personix.ArchitectureTests.Tests;

/// <summary>
/// Self-tests: verifies that CleanArchitectureRules correctly identifies violations
/// using synthetic test assemblies (or trivially passes when layers are clean).
/// </summary>
public class CleanArchitectureRulesTests
{
    [Fact]
    public void ArchitectureTestResult_Passed_IsSuccessful()
    {
        var result = ArchitectureTestResult.Passed("TestRule");

        Assert.True(result.IsSuccessful);
        Assert.Empty(result.FailingTypes);
        Assert.Equal(string.Empty, result.FailingMessage());
    }

    [Fact]
    public void ArchitectureTestResult_Failed_HasMessage()
    {
        var result = new ArchitectureTestResult("TestRule", false, ["TypeA", "TypeB"]);

        Assert.False(result.IsSuccessful);
        Assert.Contains("TypeA", result.FailingMessage());
        Assert.Contains("TestRule", result.FailingMessage());
    }

    [Fact]
    public void VerifyAll_WithMinimalAssemblies_ReturnsResults()
    {
        var assemblies = new SelfReferentialAssemblies(
            domain: typeof(ArchitectureTestResult).Assembly,
            application: typeof(CleanArchitectureRulesTests).Assembly);

        var results = CleanArchitectureRules.VerifyAll(assemblies).ToList();

        Assert.NotEmpty(results);
    }

    // Minimal implementation of ICleanArchitectureAssemblies for testing the rules themselves
    private sealed class SelfReferentialAssemblies(Assembly domain, Assembly application)
        : ICleanArchitectureAssemblies
    {
        public Assembly Domain { get; } = domain;
        public Assembly Application { get; } = application;
        public Assembly? Infrastructure => null;
        public Assembly? Persistence => null;
        public Assembly? Presentation => null;
    }
}
