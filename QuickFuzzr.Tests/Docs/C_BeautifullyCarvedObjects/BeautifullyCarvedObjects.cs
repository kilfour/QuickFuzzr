using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.C_BeautifullyCarvedObjects;

[DocFile]
public class BeautifullyCarvedObjects
{
    [Fact]
    public void Doc()
    {
        Explain.OnlyThis<BeautifullyCarvedObjects>("temp.md");
    }
}