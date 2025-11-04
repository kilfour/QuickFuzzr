using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.T_Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr.IgnoreAll()")]
public class ConfigrIgnoreAll
{
    [DocContent("**Usage:**")]
    [DocExample(typeof(ConfigrTIgnoreAll), nameof(GetConfig))]
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
        var generator =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        var thing = generator.Generate();
        Assert.Equal(0, thing.Id);
        Assert.Equal(0, thing.Prop);
    }


    [Fact]
    public void Derived()
    {
        var generator =
            from _ in GetConfig()
            from result in Fuzzr.One<DerivedThing>()
            select result;
        var thing = generator.Generate();
        Assert.Equal(0, thing.Id);
        Assert.Equal(0, thing.Prop);
        Assert.Equal(0, thing.PropOnDerived);
    }
}
