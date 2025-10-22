using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Configuration.Methods;

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
    public void IsApplied()
    {
        var generator =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, generator.Generate().Id);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfigConstant()
    {
        return Configr.Property(a => a.Name == "Id", 42);
    }

    [Fact]
    [DocContent("A utility overload exists that allows one to pass in a value instead of a fuzzr.")]
    [DocExample(typeof(ConfigrProperty), nameof(GetConfigConstant))]
    [CodeSnippet]
    public void IsApplied_Constant()
    {
        var generator =
           from _ in GetConfigConstant()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, generator.Generate().Id);
    }
}