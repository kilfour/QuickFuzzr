using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr&lt;T&gt;.Ignore(...)")]
public class ConfigrIgnoreT
{
    [DocUsage]
    [DocExample(typeof(ConfigrIgnoreT), nameof(GetFuzzr))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Person> GetFuzzr()
    {
        return
        from ignore in Configr<Person>.Ignore(a => a.Name)
        from person in Fuzzr.One<Person>()
        select person;
        // Results in => 
        // ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
    }

    [Fact]
    [DocContent("The property specified will be ignored during generation.")]
    public void StaysDefaultValue()
    {
        var result = GetFuzzr().Generate(42);
        Assert.Equal("", result.Name);
        Assert.Equal(67, result.Age);
    }


    [Fact]
    [DocContent("Derived classes generated also ignore the base property.")]
    public void WorksForDerived()
    {
        var fuzzr =
            from ignore in Configr<Person>.Ignore(a => a.Name)
            from employee in Fuzzr.One<Employee>()
            select employee;
        var result = fuzzr.Generate(42);
        Assert.Equal("", result.Name);
        Assert.Equal(26, result.Age);
        Assert.Equal("ddnegsn", result.Email);
        Assert.Equal("tg", result.SocialSecurityNumber);
    }
}
