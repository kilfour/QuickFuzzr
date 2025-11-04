using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.D_ADeepForest;

[DocFile]
public class ADeepForest
{
    [Fact]
    public void Doc()
    {
        Explain.OnlyThis<ADeepForest>("temp.md");
    }
}