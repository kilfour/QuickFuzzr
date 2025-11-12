using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr&lt;T&gt;.IgnoreAll()")]
public class ConfigrIgnoreAllT
{
    [DocUsage]
    [DocExample(typeof(ConfigrIgnoreAllT), nameof(GetFuzzr))]
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
    [DocContent("Ignore all properties while generating an object.")]
    public void StaysDefaultValue()
    {
        var (person, address) = GetFuzzr().Generate(42);
        Assert.Equal("", person.Name);
        Assert.Equal(0, person.Age);
        Assert.Equal("ddnegsn", address.Street);
        Assert.Equal("tg", address.City);
    }


    [Fact]
    [DocContent("`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.")]
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
}
