using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Configuration;


[DocFile]
[DocFileHeader("Configr<T>.IgnoreAll()")]
public class ConfigrTIgnoreAll
{
    [DocContent("**Usage:**")]
    [DocExample(typeof(ConfigrTIgnore), nameof(GetConfig))]
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
        Assert.Equal(0, generator.Generate().Id);
    }


    [Fact]
    [DocContent("`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.")]
    public void Derived()
    {
        var generator =
            from _ in GetConfig()
            from result in Fuzzr.One<DerivedThing>()
            select result;
        Assert.NotEqual(0, generator.Generate().Id);
    }
}
