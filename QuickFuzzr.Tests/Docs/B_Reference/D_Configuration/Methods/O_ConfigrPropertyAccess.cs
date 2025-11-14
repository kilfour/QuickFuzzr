using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Property Access")]
[DocContent("Control which type of properties QuickFuzzr generates.")]
[DocColumn(Configuring.Columns.Description, "Controls auto-generation for specific property access levels.")]
[DocContent(
@"\n**Signature:**
```csharp
Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) 
Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)
```")]
public class O_ConfigrPropertyAccess
{
    [DocUsage]
    [DocExample(typeof(O_ConfigrPropertyAccess), nameof(GetFuzzr))]
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
    [DocContent("Updates state flags using bitwise enable/disable semantics.")]
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
}
