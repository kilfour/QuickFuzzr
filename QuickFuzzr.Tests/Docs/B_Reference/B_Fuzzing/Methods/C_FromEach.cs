using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocColumn(Fuzzing.Columns.Description, "Creates a Fuzzr that produces an IEnumerable based on the elements of a source IEnumerable.")]
[DocContent("Creates a Fuzzr that produces an IEnumerable based on the elements of a source IEnumerable.")]
[DocSignature("FromEach<T, U>(IEnumerable<T> source, Func<T, FuzzrOf<U>> func)")]
public class C_FromEach
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<IEnumerable<int>> Example_Fuzzr()
    {
        return Fuzzr.FromEach([1, 100, 1000], a => Fuzzr.Int(1, a));
        // Results in => [ 1, 67, 141 ]
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(C_FromEach), nameof(Example_Fuzzr))]
    public void Example()
    {
        var result = Example_Fuzzr().Generate(42).ToList();
        Assert.Equal(1, result[0]);
        Assert.Equal(67, result[1]);
        Assert.Equal(141, result[2]);
    }
}