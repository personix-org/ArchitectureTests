# Common.ArchitectureTests

Shared Clean Architecture enforcement helpers built on [NetArchTest.Rules](https://github.com/BenMorris/NetArchTest).

## What's included

| Type | Purpose |
|------|---------|
| `ICleanArchitectureAssemblies` | Interface mapping your assemblies to CA layers |
| `CleanArchitectureRules` | Static methods for each standard dependency rule |
| `ArchitectureTestResult` | Value-object result with a human-readable failure message |
| `CleanArchitectureTestBase` | xUnit base class – inherit and get all rules as auto-generated Facts |

## Usage

### Option A – Inherit the base class (recommended)

```csharp
// Architecture.Tests project in your solution
internal sealed class MyLayerAssemblies : ICleanArchitectureAssemblies
{
    public Assembly Domain        => typeof(global::Domain.AssemblyMarker).Assembly;
    public Assembly Application   => typeof(global::Application.AssemblyMarker).Assembly;
    public Assembly? Infrastructure => typeof(global::Infrastructure.AssemblyMarker).Assembly;
    public Assembly? Persistence  => typeof(global::Persistence.AssemblyMarker).Assembly;
    public Assembly? Presentation => typeof(global::Presentation.AssemblyMarker).Assembly;
}

public class ArchTests : CleanArchitectureTestBase
{
    protected override ICleanArchitectureAssemblies Assemblies { get; } = new MyLayerAssemblies();
    // All 5 standard Facts are now executed automatically.
}
```

### Option B – Call rules individually

```csharp
var assemblies = new MyLayerAssemblies();
var result = CleanArchitectureRules.DomainMustNotDependOnOtherLayers(assemblies);
Assert.True(result.IsSuccessful, result.FailingMessage());
```

## Standard rules enforced

1. **Domain** must not depend on Application, Infrastructure, Persistence or Presentation
2. **Application** must not depend on Infrastructure, Persistence or Presentation
3. **Infrastructure** must not depend on Presentation
4. **Persistence** must not depend on Infrastructure
5. **Persistence** must not depend on Presentation

Rules for `null` assemblies are automatically skipped (e.g. when you have no Persistence layer).
