using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileCodeHeader("Configr<T>.IgnoreAll")]
[DocColumn(Configuring.Columns.Description, "Ignores all properties of type T.")]
[DocContent("Ignore all properties while generating an object.")]
[DocSignature("Configr<T>.IgnoreAll()")]
public class C_ConfigrIgnoreAllT
{
    [DocUsage]
    [DocExample(typeof(C_ConfigrIgnoreAllT), nameof(GetFuzzr))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(Person, Address)> GetFuzzr()
    {
        return
        from ignore in Configr<Person>.IgnoreAll()
        from person in Fuzzr.One<Person>()
        from address in Fuzzr.One<Address>()
        select (person, address);
        // Results in => 
        // ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
    }

    [Fact]
    public void StaysDefaultValue()
    {
        var (person, address) = GetFuzzr().Generate(42);
        Assert.Equal("", person.Name);
        Assert.Equal(0, person.Age);
        Assert.Equal("ddnegsn", address.Street);
        Assert.Equal("tg", address.City);
    }


    [Fact]
    [DocContent("- `IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.")]
    public void Derived()
    {
        var fuzzr =
            from _ in Configr<Person>.IgnoreAll()
            from result in Fuzzr.One<Employee>()
            select result;
        var employee = fuzzr.Generate(42);
        Assert.Equal("nij", employee.Name);
        Assert.Equal(26, employee.Age);
        Assert.Equal("ddnegsn", employee.Email);
        Assert.Equal("tg", employee.SocialSecurityNumber);
    }

    [Fact]
    public void Configr_InChain()
    {
        var fuzzr =
            from e1 in Fuzzr.One<Person>()
            from _1 in Configr<Person>.IgnoreAll()
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
            from _1 in Configr<Person>.IgnoreAll()
            from i in Fuzzr.Int()
            select i;
        var state = new State();
        fuzzr.Many(2)(state);
        Assert.Single(state.StuffToIgnoreAll);
    }
}
