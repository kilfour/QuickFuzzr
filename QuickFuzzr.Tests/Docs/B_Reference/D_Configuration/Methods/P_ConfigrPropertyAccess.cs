using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Property Access")]
[DocContent("Control which kinds of properties QuickFuzzr is allowed to populate.")]
[DocColumn(Configuring.Columns.Description, "Controls auto-generation for specific property access levels.")]
[DocContent(
@"
**Signature:**
```csharp
Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) 
Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)
```")]
public class P_ConfigrPropertyAccess
{
    [DocUsage]
    [DocExample(typeof(P_ConfigrPropertyAccess), nameof(GetFuzzr))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(PrivatePerson person1, PrivatePerson person2)> GetFuzzr()
    {
        return
        from enable in Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)
        from person1 in Fuzzr.One<PrivatePerson>()
        from disable in Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)
        from person2 in Fuzzr.One<PrivatePerson>()
        select (person1, person2);
        // Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
    }

    [Fact]
    public void Example()
    {
        var (person1, person2) = GetFuzzr().Generate(1234);
        Assert.Equal("xiyi", person1.Name);
        Assert.Equal(94, person1.Age);
        Assert.Equal("", person2.Name);
        Assert.Equal(0, person2.Age);
    }

    [Fact]
    [DocContent("- Updates state flags using bitwise enable/disable semantics.")]
    public void Updates_State_PropertyAccess_Flags()
    {
        var state = new State();
        Assert.Equal(PropertyAccess.PublicSetters, state.PropertyAccess);

        // Enable a flag
        Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)(state);
        Assert.True(state.PropertyAccess.HasFlag(PropertyAccess.PublicSetters));
        Assert.True(state.PropertyAccess.HasFlag(PropertyAccess.InitOnly));

        // Disable the same flag
        Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)(state);
        Assert.True(state.PropertyAccess.HasFlag(PropertyAccess.PublicSetters));
        Assert.False(state.PropertyAccess.HasFlag(PropertyAccess.InitOnly));
    }

    [Fact]
    [DocContent("- The default value is `PropertyAccess.PublicSetters`.")]
    public void PropertyAccess_DefaultsToPublic()
    {
        var fuzzr =
            from stringDefault in Configr.Primitive(Fuzzr.Constant("FUZZED"))
            from showCase in Fuzzr.One<PropertyShowcase>()
            select showCase;
        var result = fuzzr.Generate();
        Assert.Equal("FUZZED", result.PublicSetter);
        Assert.Equal("", result.PrivateSetter);
        Assert.Equal("", result.InitOnly);
        Assert.Equal("fixed", result.ReadOnly);
        Assert.Equal("", result.ProtectedSetter);
        Assert.Equal("", result.InternalSetter);
        Assert.Equal("hidden", result.PrivateProperty);
        Assert.Equal("fixed hidden", result.Calculated);
    }

    [Fact]
    public void PropertyAccess_DisablePublic()
    {
        var fuzzr =
            from stringDefault in Configr.Primitive(Fuzzr.Constant("FUZZED"))
            from cfg in Configr.DisablePropertyAccessFor(PropertyAccess.PublicSetters)
            from showCase in Fuzzr.One<PropertyShowcase>()
            select showCase;
        var result = fuzzr.Generate();
        Assert.Equal("", result.PublicSetter);
        Assert.Equal("", result.PrivateSetter);
        Assert.Equal("", result.InitOnly);
        Assert.Equal("fixed", result.ReadOnly);
        Assert.Equal("", result.ProtectedSetter);
        Assert.Equal("", result.InternalSetter);
        Assert.Equal("hidden", result.PrivateProperty);
        Assert.Equal("fixed hidden", result.Calculated);
    }

    private static FuzzrOf<PropertyShowcase> GetFuzzrEnabledFor(PropertyAccess propertyAccess)
    {
        return from stringDefault in Configr.Primitive(Fuzzr.Constant("FUZZED"))
               from cfg in Configr.EnablePropertyAccessFor(propertyAccess)
               from showCase in Fuzzr.One<PropertyShowcase>()
               select showCase;
    }

    [Fact]
    public void PropertyAccess_PrivateSetters()
    {
        var result = GetFuzzrEnabledFor(PropertyAccess.PrivateSetters).Generate();
        Assert.Equal("FUZZED", result.PublicSetter);
        Assert.Equal("FUZZED", result.PrivateSetter);
        Assert.Equal("", result.InitOnly);
        Assert.Equal("fixed", result.ReadOnly);
        Assert.Equal("", result.ProtectedSetter);
        Assert.Equal("", result.InternalSetter);
        Assert.Equal("hidden", result.PrivateProperty);
        Assert.Equal("fixed hidden", result.Calculated);
    }

    [Fact]
    public void PropertyAccess_InitOnly()
    {
        var result = GetFuzzrEnabledFor(PropertyAccess.InitOnly).Generate();
        Assert.Equal("FUZZED", result.PublicSetter);
        Assert.Equal("", result.PrivateSetter);
        Assert.Equal("FUZZED", result.InitOnly);
        Assert.Equal("fixed", result.ReadOnly);
        Assert.Equal("", result.ProtectedSetter);
        Assert.Equal("", result.InternalSetter);
        Assert.Equal("hidden", result.PrivateProperty);
        Assert.Equal("fixed hidden", result.Calculated);
    }

    [Fact]
    [DocContent("- `ReadOnly` only applies to get-only **auto-properties** (with a compiler-generated backing field).")]
    [DocContent("- Getter-only properties without a backing field (calculated or custom-backed) are never auto-generated.")]
    public void PropertyAccess_ReadOnly()
    {
        var result = GetFuzzrEnabledFor(PropertyAccess.ReadOnly).Generate();
        Assert.Equal("FUZZED", result.PublicSetter);
        Assert.Equal("", result.PrivateSetter);
        Assert.Equal("", result.InitOnly);
        Assert.Equal("FUZZED", result.ReadOnly);
        Assert.Equal("", result.ProtectedSetter);
        Assert.Equal("", result.InternalSetter);
        Assert.Equal("hidden", result.PrivateProperty);
        Assert.Equal("FUZZED hidden", result.Calculated);
    }

    [Fact]
    public void PropertyAccess_ProtectedSetters()
    {
        var result = GetFuzzrEnabledFor(PropertyAccess.ProtectedSetters).Generate();
        Assert.Equal("FUZZED", result.PublicSetter);
        Assert.Equal("", result.PrivateSetter);
        Assert.Equal("", result.InitOnly);
        Assert.Equal("fixed", result.ReadOnly);
        Assert.Equal("FUZZED", result.ProtectedSetter);
        Assert.Equal("", result.InternalSetter);
        Assert.Equal("hidden", result.PrivateProperty);
        Assert.Equal("fixed hidden", result.Calculated);
    }

    [Fact]
    public void PropertyAccess_InternalSetters()
    {
        var result = GetFuzzrEnabledFor(PropertyAccess.InternalSetters).Generate();
        Assert.Equal("FUZZED", result.PublicSetter);
        Assert.Equal("", result.PrivateSetter);
        Assert.Equal("", result.InitOnly);
        Assert.Equal("fixed", result.ReadOnly);
        Assert.Equal("", result.ProtectedSetter);
        Assert.Equal("FUZZED", result.InternalSetter);
        Assert.Equal("hidden", result.PrivateProperty);
        Assert.Equal("fixed hidden", result.Calculated);
    }
}
