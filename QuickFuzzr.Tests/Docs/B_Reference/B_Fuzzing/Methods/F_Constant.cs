using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocColumn(Fuzzing.Columns.Description, "Wraps a fixed value in a Fuzzr, producing the same result every time.")]
[DocContent(
@"This Fuzzr wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined Fuzzrs.")]
[DocSignature("Fuzzr.Constant(T value)")]
public class F_Constant
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<int> Returns_Value_Fuzzr()
    {
        return Fuzzr.Constant(42);
        // Results in => 42
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(F_Constant), nameof(Returns_Value_Fuzzr))]
    public void Returns_Value()
    {
        Assert.Equal(42, Returns_Value_Fuzzr().Generate());
    }

    [Fact]
    public void Null_Can_Be_A_Constant()
    {
        Assert.Null(Fuzzr.Constant((string)null!).Generate());
    }
}