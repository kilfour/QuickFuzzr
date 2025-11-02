using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Objects;

[DocFile]
public class BeautifullyCarvedObjects
{
    [Fact(Skip = "wip")]
    [DocContent(
@"As seen in the previous chapter, `Fuzzr.One<T>()` is the cannonical way to get a random instance of a class.  
This works fine for domain models that have properties with public setters,
which is what a lot of the domains I come across in the dotnet eco-system these days look like.  

But for those who like a bit of encapsulation now and then ...")]
    public void DocIt()
    {
        Explain.OnlyThis<BeautifullyCarvedObjects>("temp.md");
    }
}