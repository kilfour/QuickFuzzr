using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileCodeHeader("Configr<T>.Ignore")]
[DocColumn(Configuring.Columns.Description, "Ignores one specific property on type T during generation.")]
[DocContent("The property specified will be ignored during generation.")]
[DocSignature("Configr<T>.Ignore(Expression<Func<T, TProperty>> expression)")]
public class A_ConfigrIgnoreT
{
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
    [DocUsage]
    [DocExample(typeof(A_ConfigrIgnoreT), nameof(GetFuzzr))]
    public void StaysDefaultValue()
    {
        var result = GetFuzzr().Generate(42);
        Assert.Equal("", result.Name);
        Assert.Equal(67, result.Age);
    }


    [Fact]
    [DocContent("- Derived classes generated also ignore the base property.")]
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

    [Fact]
    [DocExceptions]
    [DocContent("  - `ArgumentNullException`: When the expression is `null`.")]
    public void Null()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => Configr<Person>.Ignore<string>(null!));
        Assert.Equal("Value cannot be null. (Parameter 'expression')", ex.Message);
    }

    [Fact]
    public void Configr_InChain()
    {
        var fuzzr =
            from e1 in Fuzzr.One<Person>()
            from _1 in Configr<Person>.Ignore(p => p.Name)
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
            from _1 in Configr<Person>.Ignore(p => p.Name)
            from i in Fuzzr.Int()
            select i;
        var state = new State();
        fuzzr.Many(2)(state);
        Assert.Single(state.StuffToIgnore);
    }
}
