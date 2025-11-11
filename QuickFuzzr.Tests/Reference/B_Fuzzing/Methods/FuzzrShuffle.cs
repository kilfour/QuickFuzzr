using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.Shuffle&lt;T&gt;()")]
[DocColumn(Fuzzing.Columns.Description, "Creates a generator that randomly shuffles an input sequence.")]
public class FuzzrShuffle
{
    [Fact]
    public void Spike()
    {
        var result = Fuzzr.Shuffle("John", "Paul", "George", "Ringo")
            .Generate(42)
            .ToArray();
        Assert.Equal(["Paul", "Ringo", "John", "George"], result);
    }
}