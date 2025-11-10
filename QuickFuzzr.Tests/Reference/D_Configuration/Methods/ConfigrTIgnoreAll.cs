using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr&lt;T&gt;.IgnoreAll()")]
public class ConfigrTIgnoreAll
{
    [DocUsage]
    [DocExample(typeof(ConfigrTIgnoreAll), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr<Thing>.IgnoreAll();
    }

    [Fact]
    [DocContent("Ignore all properties while generating an object.")]
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
    [DocContent("`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.")]
    public void Derived()
    {
        var generator =
            from _ in GetConfig()
            from result in Fuzzr.One<DerivedThing>()
            select result;
        var thing = generator.Generate();
        Assert.NotEqual(0, thing.Id);
        Assert.NotEqual(0, thing.Prop);
        Assert.NotEqual(0, thing.PropOnDerived);
    }
}
