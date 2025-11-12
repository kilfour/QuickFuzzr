using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr.IgnoreAll()")]
public class ConfigrIgnoreAll
{
    [DocUsage]
    [DocExample(typeof(ConfigrIgnoreAllT), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.IgnoreAll();
    }

    [Fact]
    [DocContent("Ignore all properties while generating anything.")]
    public void StaysDefaultValue()
    {
        var fuzzr =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        var thing = fuzzr.Generate();
        Assert.Equal(0, thing.Id);
        Assert.Equal(0, thing.Prop);
    }


    [Fact]
    public void Derived()
    {
        var fuzzr =
            from _ in GetConfig()
            from result in Fuzzr.One<DerivedThing>()
            select result;
        var thing = fuzzr.Generate();
        Assert.Equal(0, thing.Id);
        Assert.Equal(0, thing.Prop);
        Assert.Equal(0, thing.PropOnDerived);
    }
}
