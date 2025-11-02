using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Objects;

public class BeautifullyCarvedObjets
{
    [Fact]
    public void DocIt()
    {
        Explain.OnlyThis<BeautifullyCarvedObjets>("temp.md");
    }
}