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

    // TODO : implement tests in the same vein as the other test in the B_Reference namespace for 
    // - ExtFuzzr.Shuffle
}