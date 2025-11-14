using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.Property")]
[DocContent("Any property matching the predicate will use the specified Fuzzr during generation.")]
[DocSignature("Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)")]
public class F_ConfigrProperty
{
    [DocUsage]
    [DocExample(typeof(F_ConfigrProperty), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.Property(a => a.Name == "Id", Fuzzr.Constant(42));
    }

    [Fact]
    public void IsApplied()
    {
        var fuzzr =
           from _ in GetConfig()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfigConstant()
    {
        return Configr.Property(a => a.Name == "Id", 42);
    }

    [Fact]
    [DocContent("A utility overload exists that allows one to pass in a value instead of a fuzzr.")]
    [DocExample(typeof(F_ConfigrProperty), nameof(GetConfigConstant))]
    [CodeSnippet]
    public void IsApplied_Constant()
    {
        var fuzzr =
           from _ in GetConfigConstant()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfigFactory()
    {
        return Configr.Property(a => a.Name == "Id", a => Fuzzr.Constant(42));
    }

    [Fact]
    [DocContent("Another overload allows you to create a fuzzr dynamically using a `Func<PropertyInfo, FuzzrOf<T>>` factory method.")]
    [DocExample(typeof(F_ConfigrProperty), nameof(GetConfigFactory))]
    public void IsApplied_Factory()
    {
        var fuzzr =
           from _ in GetConfigFactory()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfigFactory_Constant()
    {
        return Configr.Property(a => a.Name == "Id", a => 42);
    }

    [Fact]
    [DocContent("With the same *pass in a value* conveniance helper.")]
    [DocExample(typeof(F_ConfigrProperty), nameof(GetConfigFactory_Constant))]
    public void IsApplied_Factory_Constant()
    {
        var fuzzr =
           from _ in GetConfigFactory_Constant()
           from result in Fuzzr.One<Thing>()
           select result;
        Assert.Equal(42, fuzzr.Generate().Id);
    }
}