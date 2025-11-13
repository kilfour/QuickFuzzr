using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr.IgnoreAll()")]
public class D_ConfigrIgnoreAll
{

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(Person, Address)> GetFuzzr()
    {
        return
        from ignore in Configr.IgnoreAll()
        from person in Fuzzr.One<Person>()
        from address in Fuzzr.One<Address>()
        select (person, address);
        // Results in => 
        // ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
    }

    [Fact]
    [DocContent("Ignore all properties while generating anything.")]
    [DocUsage]
    [DocExample(typeof(D_ConfigrIgnoreAll), nameof(GetFuzzr))]
    public void StaysDefaultValue()
    {
        var (person, address) = GetFuzzr().Generate(42);
        Assert.Equal(string.Empty, person.Name);
        Assert.Equal(0, person.Age);
        Assert.Equal(string.Empty, address.Street);
        Assert.Equal(string.Empty, address.City);
    }
}
