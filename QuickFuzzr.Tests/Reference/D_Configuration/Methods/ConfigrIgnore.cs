using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.Ignore(...)")]
public class ConfigrIgnore
{
    [DocUsage]
    [DocExample(typeof(ConfigrIgnore), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.Ignore(a => a.Name == "Id");
    }

    [Fact]
    [DocContent("Any property matching the predicate will be ignored during generation.")]
    public void StaysDefaultValue()
    {
        var generator =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(0, generator.Generate().Id);
    }
}
