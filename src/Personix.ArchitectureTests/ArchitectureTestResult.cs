namespace Personix.ArchitectureTests;

/// <summary>
/// Result of a single architecture rule evaluation.
/// </summary>
/// <param name="RuleName">Name of the rule that was evaluated.</param>
/// <param name="IsSuccessful">Whether the rule passed.</param>
/// <param name="FailingTypes">Types that violated the rule (empty on success).</param>
public sealed record ArchitectureTestResult(
    string RuleName,
    bool IsSuccessful,
    IReadOnlyList<string> FailingTypes)
{
    /// <summary>Creates a passing result with no violations.</summary>
    public static ArchitectureTestResult Passed(string ruleName) =>
        new(ruleName, true, []);

    /// <summary>
    /// Returns a human-readable failure message, or an empty string when successful.
    /// </summary>
    public string FailingMessage() =>
        IsSuccessful
            ? string.Empty
            : $"Rule '{RuleName}' failed. Violating types: {string.Join(", ", FailingTypes)}";
}
