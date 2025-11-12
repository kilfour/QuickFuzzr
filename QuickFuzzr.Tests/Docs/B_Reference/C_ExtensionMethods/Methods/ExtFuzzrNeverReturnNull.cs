using QuickFuzzr.Tests._Tools;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("ExtFuzzr.NeverReturnNull(this FuzzrOf<T?> fuzzr)")]
public class ExtFuzzrNeverReturnNull
{

    [Fact]
    [DocContent("Filters out nulls from a nullable fuzzr, retrying up to the retry limit.")]
    [DocUsage]
    public void Produces_NonNulls()
    {
        var fuzzr = Fuzzr.Int().Nullable(0.5).NeverReturnNull();
        var value = fuzzr.Generate(42);
        Assert.True(value.HasValue);
    }

    [Fact]
    [DocExceptions]
    [DocException("NonNullValueExhaustedException", "When all attempts result in null.")]
    public void Throws_When_AlwaysNull()
    {
        var fuzzr = Fuzzr.Int().Nullable(1.0).NeverReturnNull();
        var ex = Assert.Throws<NonNullValueExhaustedException>(() => fuzzr.Generate());
    }
}
