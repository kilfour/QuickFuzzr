using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Configuration;


[DocFile]
[DocFileHeader("Configr<T>.Ignore(...)")]
public class ConfigrTIgnore
{
    [DocContent("**Usage:**")]
    [DocExample(typeof(ConfigrTIgnore), nameof(GetConfig))]
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
        var generator =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(0, generator.Generate().Id);
    }


    [Fact]
    [DocContent("Derived classes generated also ignore the base property.")]
    public void WorksForDerived()
    {
        var generator =
            from _ in GetConfig()
            from result in Fuzzr.One<DerivedThing>()
            select result;
        Assert.Equal(0, generator.Generate().Id);
    }
}
