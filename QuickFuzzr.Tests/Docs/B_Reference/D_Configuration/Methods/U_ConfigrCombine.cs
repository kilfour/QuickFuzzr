using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.Combine")]
[DocColumn(Configuring.Columns.Description, "Combines multiple configuration operations into one.")]
[DocContent(
@"Combines multiple configuration operations into a single Configr.  
The operations are applied in argument order.")]
[DocSignature("Configr.Combine(params FuzzrOf<Intent>[] configrs)")]
public class U_ConfigrCombine
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.Combine(
            Configr<Person>.Property(person => person.Name, "Arthur"),
            Configr<Person>.Property(person => person.Age, 42));
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(U_ConfigrCombine), nameof(GetConfig))]
    public void Applies_All_Configurations()
    {
        var fuzzr =
            from _ in GetConfig()
            from person in Fuzzr.One<Person>()
            select person;

        var result = fuzzr.Generate();

        Assert.Equal("Arthur", result.Name);
        Assert.Equal(42, result.Age);
    }

    [Fact]
    [DocContent("- Configurations are applied in argument order, so later operations can override earlier ones.")]
    public void Applies_Configurations_In_Argument_Order()
    {
        var fuzzr =
            from _ in Configr.Combine(
                Configr<Person>.Property(person => person.Age, 1),
                Configr<Person>.Property(person => person.Age, 2))
            from person in Fuzzr.One<Person>()
            select person;

        var result = fuzzr.Generate();

        Assert.Equal(2, result.Age);
    }

    [Fact]
    [DocContent("- With no arguments, `Configr.Combine()` has no effect.")]
    public void Empty_Combination_Has_No_Effect()
    {
        var fuzzr =
            from _ in Configr.Combine()
            from value in Fuzzr.Constant(42)
            select value;

        Assert.Equal(42, fuzzr.Generate());
    }

    [Fact]
    [DocExceptions]
    [DocException("ArgumentNullException", "When the provided configuration array is null.")]
    public void Null_Configurations_Throws()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Configr.Combine(null!));

        Assert.Equal("Value cannot be null. (Parameter 'configrs')", ex.Message);
    }

    [Fact]
    [DocException("ArgumentNullException", "When a configuration in the array is null.")]
    public void Null_Configuration_Throws_On_Invocation()
    {
        var configr = Configr.Combine(Configr.IgnoreAll(), null!);

        var ex = Assert.Throws<ArgumentNullException>(() => configr.Generate());

        Assert.Equal("Value cannot be null. (Parameter 'configr')", ex.Message);
    }
}
