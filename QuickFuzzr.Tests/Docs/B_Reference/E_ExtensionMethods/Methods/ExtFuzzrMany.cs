using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("Many")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Produces a number of values from the wrapped Fuzzr.")]
[DocContent("Produces a fixed number of values from a Fuzzr.")]
[DocSignature("ExtFuzzr.Many(this FuzzrOf<T> fuzzr, int number)")]
public class ExtFuzzrMany
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<IEnumerable<int>> Fixed_Count_Fuzzr()
    {
        return Fuzzr.Constant(6).Many(3);
        // Results in => [6, 6, 6]
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ExtFuzzrMany), nameof(Fixed_Count_Fuzzr))]
    public void Fixed_Count()
    {
        var values = Fixed_Count_Fuzzr().Generate().ToArray();
        Assert.Equal(3, values.Length);
        Assert.True(values.All(v => v == 6));
    }

    [DocOverloads]
    [DocOverload("Many(this FuzzrOf<T> fuzzr, int min, int max)")]
    [DocContent("  Produces a variable number of values within bounds.")]
    [Fact]
    public void Range_Count()
    {
        var values = Fuzzr.Int().Many(1, 3).Generate(42).ToArray();
        Assert.InRange(values.Length, 1, 3);

        CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.Int().Many(1, 3),
            ("Count == 1", numbers => numbers.Count() == 1),
            ("Count == 2", numbers => numbers.Count() == 2),
            ("Count == 3", numbers => numbers.Count() == 3));
    }
}
