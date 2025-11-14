using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileCodeHeader("Configr<T>.Apply")]
[DocColumn(Configuring.Columns.Description, "Registers an action executed for each generated value of type `T`.")]
[DocContent("Registers an action executed for each generated value of type `T` without modifying the value itself. Use for performing operations like logging, adding to collections, or calling methods that have side effects but don't transform the data.")]
[DocSignature("Configr<T>.Apply(Action<T> action)")]
public class L_ConfigrApplyT
{
    [CodeSnippet]
    [CodeRemove("42")]
    [CodeRemove("return seen;")]
    private static List<Person> ExampleGetResult()
    {
        var seen = new List<Person>();
        var fuzzr =
            from look in Configr<Person>.Apply(seen.Add)
            from person in Fuzzr.One<Person>()
            from employee in Fuzzr.One<Employee>()
            select (person, employee);
        var value = fuzzr.Generate(42);
        // seen now equals 
        // [ ( 
        //     Person { Name: "ddnegsn", Age: 18 },
        //     Employee { Email: "ggnijgna", SocialSecurityNumber: "pkdcsvobs", Name: "xs", Age: 52 }
        //) ]
        return seen;
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(L_ConfigrApplyT), nameof(ExampleGetResult))]
    public void Example()
    {
        var result = ExampleGetResult();
        Assert.Equal("ddnegsn", result[0].Name);
        Assert.Equal(18, result[0].Age);
        Assert.Equal("xs", result[1].Name);
        Assert.Equal(52, result[1].Age);
    }

    [Fact]
    [DocExceptions]
    [DocException("NullReferenceException", "When the provided Action is null and later invoked.")]
    public void Null_Action_Throws_On_Invocation()
    {
        var fuzzr =
            from cfg in Configr<Person>.Apply(null!)
            from person in Fuzzr.One<Person>()
            select person;
        var ex = Assert.Throws<NullReferenceException>(() => fuzzr.Generate());
        Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
    }

    [Fact]
    public void Check_For_Too_Many_Applies()
    {
        var seen = new List<Person>();
        var fuzzr =
            from look in Configr<Person>.Apply(seen.Add)
            from person in Fuzzr.One<Person>()
            from employee in Fuzzr.One<Employee>()
            select (person, employee);
        var value = fuzzr.Many(2).Generate(42);
        Assert.Equal(4, seen.Count);
    }
}
