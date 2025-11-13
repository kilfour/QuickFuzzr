using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileCodeHeader("Fuzzr.Shuffle<T>()")]
[DocColumn(Fuzzing.Columns.Description, "Creates a fuzzr that randomly shuffles an input sequence.")]
[DocContent(
@"Creates a fuzzr that produces a random permutation of the provided sequence.  
Use for randomized ordering, unbiased sampling without replacement.
")]
public class C_FuzzrShuffle
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
    [DocExample(typeof(C_FuzzrShuffle), nameof(Shuffle_Example_Fuzzr))]
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
@"Value cannot be null. (Parameter 'source')"; // Check: Update Message
}