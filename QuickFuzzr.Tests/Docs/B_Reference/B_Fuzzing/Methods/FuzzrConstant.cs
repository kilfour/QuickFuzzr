using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.Constant&lt;T&gt;(T value)")]
[DocColumn(Fuzzing.Columns.Description, "Wraps a fixed value in a generator, producing the same result every time.")]
public class FuzzrConstant
{
    [Fact]
    [DocContent(
@"This generator wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined generators.")]
    public void JustReturnsValue()
    {
        Assert.Equal(42, Fuzzr.Constant(42).Generate());
    }
}