using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr.IgnoreAll")]
[DocColumn(Configuring.Columns.Description, "Disables auto-generation for all properties on all types.")]
[DocContent("Ignore all properties while generating anything.")]
[DocSignature("Configr.IgnoreAll()")]
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

    [Fact]
    public void Configr_InChain()
    {
        var fuzzr =
            from e1 in Fuzzr.One<Person>()
            from _1 in Configr.IgnoreAll()
            from e2 in Fuzzr.One<Person>()
            select (e1, e2);
        var result = fuzzr.Generate(42);
        Assert.Equal("ddnegsn", result.e1.Name);
        Assert.Equal("", result.e2.Name);
    }

    [Fact]
    public void Configr_DoesNotMultiply()
    {
        var fuzzr =
            from _1 in Configr.IgnoreAll()
            from i in Fuzzr.Int()
            select i;
        var state = new State();
        fuzzr.Many(2)(state);
        Assert.Single(state.GeneralStuffToIgnore);
    }
}
