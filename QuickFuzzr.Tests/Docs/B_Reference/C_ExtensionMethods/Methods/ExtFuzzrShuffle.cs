using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
public class ExtFuzzrShuffle
{
    [Fact]
    public void Spike()
    {
        var result = Fuzzr.Constant((string[])["John", "Paul", "George", "Ringo"])
            .Shuffle()
            .Generate(42)
            .ToArray();
        Assert.Equal(["Paul", "Ringo", "John", "George"], result);
    }

    [Fact]
    public void Preserves_Elements()
    {
        var source = new[] { "a", "b", "c", "d" };
        var shuffled = Fuzzr.Constant((IEnumerable<string>)source).Shuffle().Generate(5).ToArray();
        Assert.Equal(source.Length, shuffled.Length);
        Assert.True(source.All(shuffled.Contains));
    }
}
