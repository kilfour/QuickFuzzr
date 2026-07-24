using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.EnableFieldAccess")]
[DocColumn(Configuring.Columns.Description, "Enables auto-generation for public mutable fields.")]
[DocContent("Field access is opt-in. By default, QuickFuzzr only populates properties.")]
[DocSignature("Configr.EnableFieldAccess()")]
public class Q_ConfigrFieldAccess
{
    [DocUsage]
    [DocExample(typeof(Q_ConfigrFieldAccess), nameof(GetFuzzr))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<PersonOutInTheFields> GetFuzzr()
    {
        return
            from _ in Configr.EnableFieldAccess()
            from person in Fuzzr.One<PersonOutInTheFields>()
            select person;
    }

    [Fact]
    public void PublicFieldsAreIgnoredByDefault()
    {
        var result = Fuzzr.One<PersonOutInTheFields>().Generate();

        Assert.Equal(string.Empty, result.Name);
        Assert.Equal(0, result.Age);
    }

    [Fact]
    [DocContent("- Populates public instance fields.")]
    [DocContent("- Static, constant, readonly, and non-public fields are not populated.")]
    public void EnablesPublicMutableFields()
    {
        var fuzzr =
            from _1 in Configr.Primitive(Fuzzr.Constant("FUZZED"))
            from _2 in Configr.Primitive(Fuzzr.Constant(42))
            from _3 in Configr.EnableFieldAccess()
            from person in Fuzzr.One<PersonOutInTheFields>()
            select person;

        var result = fuzzr.Generate();

        Assert.Equal("FUZZED", result.Name);
        Assert.Equal(42, result.Age);
    }

    [Fact]
    public void UpdatesState()
    {
        var state = new State();
        Assert.False(state.FieldAccessEnabled);

        Configr.EnableFieldAccess()(state);

        Assert.True(state.FieldAccessEnabled);
    }
}
