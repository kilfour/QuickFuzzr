using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("Shuffle")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Randomly shuffles the sequence produced by the source fuzzr.")]
[DocContent("Randomly shuffles the sequence produced by the source fuzzr.")]
[DocSignature("ExFuzzr.Shuffle<T>(this FuzzrOf<IEnumerable<T>> source)")]
public class ExtFuzzrShuffle
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<IEnumerable<int>> Shuffle_Fuzzr()
    {
        return Fuzzr.Counter("num").Many(4).Shuffle();
        // Results in => [ 2, 4, 1, 3 ]
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ExtFuzzrShuffle), nameof(Shuffle_Fuzzr))]
    public void Example()
    {
        var result = Shuffle_Fuzzr().Generate(42).ToArray();
        Assert.Equal([2, 4, 1, 3], result);
    }

    [Fact]
    [DocContent("- Preserves the elements of the source sequence.")]
    public void Preserves_Elements()
    {
        var source = new[] { "a", "b", "c", "d" };
        var shuffled = Fuzzr.Constant((IEnumerable<string>)source).Shuffle().Generate(5).ToArray();
        Assert.Equal(["a", "b", "c", "d"], source);
        Assert.True(source.All(shuffled.Contains));
    }
}
