using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Configuration;

[DocFile]
[DocFileHeader("Configr.Property(...)")]
public class ConfigrProperty
{
    [DocContent("**Usage:**")]
    [DocExample(typeof(ConfigrProperty), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.Property(a => a.Name == "Id", Fuzzr.Constant(42));
    }

    [Fact]
    [DocContent("Any property matching the predicate will use the specified Fuzzr during generation.")]
    public void StaysDefaultValue()
    {
        var generator =
           from _ in Configr<Thing>.Ignore(s => s.Id)
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(0, generator.Generate().Id);
    }
}