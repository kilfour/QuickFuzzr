using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocColumn(Fuzzing.Columns.Description, "Creates a Fuzzr that combines two source Fuzzrs into a tuple of their generated values.")]
[DocContent(
@"Creates a Fuzzr that combines two source Fuzzrs into a tuple of their generated values.")]
[DocSignature("Tuple<T1, T2>(FuzzrOf<T1> fuzzrOfT1, FuzzrOf<T2> fuzzrOfT2)")]
public class K_Tuple
{
    [CodeSnippet]
    private static FuzzrOf<(int, int)> Usage() =>
        Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int());

    [Fact]
    [DocUsage]
    [DocExample(typeof(K_Tuple), nameof(Usage))]
    public void Returns_Value()
    {
        var result = Usage().Generate(42);
        Assert.Equal(67, result.Item1);
        Assert.Equal(14, result.Item2);
    }

    [Fact]
    [DocContent("The `Tuple(...)` method supports up to seven source Fuzzrs.")]
    public void Overloads()
    {
        Assert.IsType<(int, int, int)>(
            Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int())
                .Generate());
        Assert.IsType<(int, int, int, int)>(
            Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int())
                .Generate());
        Assert.IsType<(int, int, int, int, int)>(
            Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int())
                .Generate());
        Assert.IsType<(int, int, int, int, int, int)>(
            Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int())
                .Generate());
        Assert.IsType<(int, int, int, int, int, int, int)>(
            Fuzzr.Tuple(Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int(), Fuzzr.Int())
                .Generate());
    }
}