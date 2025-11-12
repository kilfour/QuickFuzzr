using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr&lt;T&gt;.Ignore(...)")]
public class ConfigrIgnoreT
{
    [DocUsage]
    [DocExample(typeof(ConfigrIgnoreT), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr<Thing>.Ignore(s => s.Id);
    }

    [Fact]
    [DocContent("The property specified will be ignored during generation.")]
    public void StaysDefaultValue()
    {
        var fuzzr =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(0, fuzzr.Generate().Id);
    }


    [Fact]
    [DocContent("Derived classes generated also ignore the base property.")]
    public void WorksForDerived()
    {
        var fuzzr =
            from _ in GetConfig()
            from result in Fuzzr.One<DerivedThing>()
            select result;
        Assert.Equal(0, fuzzr.Generate().Id);
    }
}
