using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("ExtFuzzr.Many(this FuzzrOf<T> fuzzr, int number)")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Produces a number of values from the wrapped fuzzr.")]
public class ExtFuzzrMany
{
    [Fact]
    [DocContent("Produces a fixed number of values from a fuzzr.")]
    public void Fixed_Count()
    {
        var values = Fuzzr.Constant(7).Many(3).Generate().ToArray();
        Assert.Equal(3, values.Length);
        Assert.True(values.All(v => v == 7));
    }

    [DocOverloads]
    [DocOverload("ExtFuzzr.Many(this FuzzrOf<T> fuzzr, int min, int max)")]
    [DocContent("  Produces a variable number of values within bounds.")]
    [Fact]
    public void Range_Count()
    {
        var values = Fuzzr.Int().Many(1, 3).Generate(42).ToArray();
        Assert.InRange(values.Length, 1, 3);
    }
}
