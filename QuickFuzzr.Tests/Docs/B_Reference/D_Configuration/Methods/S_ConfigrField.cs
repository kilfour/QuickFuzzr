using System.Reflection;
using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.Field")]
[DocColumn(Configuring.Columns.Description, "Applies a custom Fuzzr or value to matching public fields across all types.")]
[DocContent("Any public field matching the predicate uses the configured Fuzzr.")]
[DocSignature("Configr.Field<TField>(Func<FieldInfo, bool> predicate, FuzzrOf<TField> fuzzr)")]
public class S_ConfigrField
{
    [DocUsage]
    [DocExample(typeof(S_ConfigrField), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.Field(field => field.Name == "Age", Fuzzr.Constant(42));
    }

    [Fact]
    public void ConfiguresMatchingField()
    {
        var fuzzr =
            from _ in GetConfig()
            from person in Fuzzr.One<PersonOutInTheFields>()
            select person;

        Assert.Equal(42, fuzzr.Generate().Age);
    }

    [Fact]
    [DocOverloads]
    [DocOverload("Configr.Field<TField>(Func<FieldInfo, bool> predicate, TField value)")]
    [DocOverload("Configr.Field<TField>(Func<FieldInfo, bool> predicate, Func<FieldInfo, FuzzrOf<TField>> factory)")]
    [DocOverload("Configr.Field<TField>(Func<FieldInfo, bool> predicate, Func<FieldInfo, TField> factory)")]
    public void SupportsValueAndMetadataFactories()
    {
        var fuzzr =
            from _1 in Configr.Field(field => field.Name == "Name", "fixed")
            from _2 in Configr.Field<int>(
                field => field.Name == "Age",
                field => Fuzzr.Constant(field.Name.Length))
            from person in Fuzzr.One<PersonOutInTheFields>()
            select person;

        var result = fuzzr.Generate();
        Assert.Equal("fixed", result.Name);
        Assert.Equal(3, result.Age);
    }

    [Fact]
    public void LatestMatchingConfigurationWins()
    {
        var fuzzr =
            from _1 in Configr.Field(field => field.Name == "Age", 1)
            from _2 in Configr.Field(field => field.Name == "Age", 2)
            from person in Fuzzr.One<PersonOutInTheFields>()
            select person;

        Assert.Equal(2, fuzzr.Generate().Age);
    }

    [Fact]
    [DocExceptions]
    [DocException("ArgumentNullException", "When the predicate, Fuzzr, or factory is `null`.")]
    public void RejectsNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => Configr.Field(null!, 42));
        Assert.Throws<ArgumentNullException>(
            () => Configr.Field(field => field.Name == "Age", (FuzzrOf<int>)null!));
        Assert.Throws<ArgumentNullException>(
            () => Configr.Field(
                field => field.Name == "Age",
                (Func<FieldInfo, int>)null!));
    }
}
