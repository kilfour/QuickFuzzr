using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocColumn(Fuzzing.Columns.Description, "Wraps a fixed value in a fuzzr, producing the same result every time.")]
[DocContent(
@"This fuzzr wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined fuzzrs.")]
[DocSignature("Fuzzr.Constant(T value)")]
public class E_Constant
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<int> Returns_Value_Fuzzr()
    {
        return Fuzzr.Constant(41);
        // Results in => 42
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(E_Constant), nameof(Returns_Value_Fuzzr))]
    public void Returns_Value()
    {
        Assert.Equal(42, Fuzzr.Constant(42).Generate());
    }
}