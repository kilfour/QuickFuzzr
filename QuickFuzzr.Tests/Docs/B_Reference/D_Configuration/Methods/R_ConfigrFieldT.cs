using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileCodeHeader("Configr<T>.Field")]
[DocColumn(Configuring.Columns.Description, "Sets a custom Fuzzr or value for one public field on type T.")]
[DocContent("Explicitly configures a public field. This does not require `Configr.EnableFieldAccess()`.")]
[DocSignature("Configr<T>.Field<TField>(Expression<Func<T, TField>> expression, FuzzrOf<TField> fuzzr)")]
public class R_ConfigrFieldT
{
    [DocUsage]
    [DocExample(typeof(R_ConfigrFieldT), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr<PersonOutInTheFields>.Field(person => person.Age, Fuzzr.Constant(42));
    }

    [Fact]
    public void ConfiguresFieldWithoutEnablingAutomaticFieldAccess()
    {
        var fuzzr =
            from _ in GetConfig()
            from person in Fuzzr.One<PersonOutInTheFields>()
            select person;

        var result = fuzzr.Generate();

        Assert.Equal(42, result.Age);
        Assert.Equal(string.Empty, result.Name);
    }

    [Fact]
    [DocOverloads]
    [DocOverload("Configr<T>.Field<TField>(Expression<Func<T, TField>> expression, TField value)")]
    [DocContent("  Allows for passing a value instead of a Fuzzr.")]
    public void ConfiguresConstantValue()
    {
        var fuzzr =
            from _ in Configr<PersonOutInTheFields>.Field(person => person.Name, "Keith")
            from person in Fuzzr.One<PersonOutInTheFields>()
            select person;

        Assert.Equal("Keith", fuzzr.Generate().Name);
    }

    [Fact]
    [DocExceptions]
    [DocException("FieldConfigurationException", "When the expression points to something other than a field.")]
    public void RejectsPropertyExpression()
    {
        Assert.Throws<FieldConfigurationException>(
            () => Configr<Person>.Field(person => person.Age, 42));
    }

    [Fact]
    [DocException("ArgumentNullException", "When the expression or Fuzzr is `null`.")]
    public void RejectsNullArguments()
    {
        Assert.Throws<ArgumentNullException>(
            () => Configr<PersonOutInTheFields>.Field<int>(null!, 42));
        Assert.Throws<ArgumentNullException>(
            () => Configr<PersonOutInTheFields>.Field(person => person.Age, null!));
    }
}
