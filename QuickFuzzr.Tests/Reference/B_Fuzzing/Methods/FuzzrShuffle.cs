using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
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