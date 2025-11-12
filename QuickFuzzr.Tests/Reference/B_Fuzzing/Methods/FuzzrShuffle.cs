using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.Shuffle&lt;T&gt;()")]
[DocColumn(Fuzzing.Columns.Description, "Creates a generator that randomly shuffles an input sequence.")]
[DocContent(
@"Creates a generator that produces a random permutation of the provided sequence.  
Use for randomized ordering, unbiased sampling without replacement.")]
public class FuzzrShuffle
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<IEnumerable<string>> Shuffle_Example_Fuzzr()
    {
        return Fuzzr.Shuffle("John", "Paul", "George", "Ringo");
        // Results in => ["Paul", "Ringo", "John", "George"]
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(FuzzrShuffle), nameof(Shuffle_Example_Fuzzr))]
    public void Shuffle_Example()
    {
        var result = Shuffle_Example_Fuzzr()
            .Generate(42)
            .ToArray();
        Assert.Equal(["Paul", "Ringo", "John", "George"], result);
    }

    [Fact]
    [DocOverloads]
    [DocContent("- `Shuffle<T>(IEnumerable<T> values)`:")]
    [DocContent("  Same as above, but accepts any enumerable source.")]
    public void Enumerable_Example()
    {
        IEnumerable<string> list = ["John", "Paul", "George", "Ringo"];
        var result = Fuzzr.Shuffle(list).Generate(42);
        Assert.Equal(["Paul", "Ringo", "John", "George"], result);
    }

    [Fact]
    [DocExceptions]
    [DocContent("  - `ArgumentNullException`: When the input collection is `null`.")]
    public void Null_Values_Throws()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Fuzzr.Shuffle<int>(null!).Generate());
        Assert.Equal(Null_Values_Message(), ex.Message);
    }

    private static string Null_Values_Message() =>
@"Value cannot be null. (Parameter 'source')"; // TODO: Update Message
}