using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.Constant&lt;T&gt;(T value)")]
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