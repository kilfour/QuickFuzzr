using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;

[DocFile]
[DocFileHeader("Configr.Primitive&lt;T&gt;(this FuzzrOf&lt;T&gt; fuzzr)")]
[DocContent(
@"Registers a global default fuzzr for primitive types.
Use this to override how QuickFuzzr generates built-in types across all automatically created objects.
")]
public class ConfigrPrimitive
{

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(Person, TimeSlot)> GetFuzzr()
    {
        return
        from cfgInt in Configr.Primitive(Fuzzr.Constant(42))
        from person in Fuzzr.One<Person>()
        from timeslot in Fuzzr.One<TimeSlot>()
        select (person, timeslot);
        // Results in => 
        // ( Person { Name: "ddnegsn", Age: 42 }, TimeSlot { Day: Monday, Time: 42 } )
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ConfigrPrimitive), nameof(GetFuzzr))]
    public void IsApplied()
    {
        var (person, timeslot) = GetFuzzr().Generate(42);
        Assert.Equal("ddnegsn", person.Name);
        Assert.Equal(42, person.Age);
        Assert.Equal(DayOfWeek.Monday, timeslot.Day);
        Assert.Equal(42, timeslot.Time);
    }

    [Fact]
    [DocContent("Replacing a primitive fuzzr automatically impacts its nullable counterpart.")]
    public void NullableIsApplied()
    {
        var fuzzr =
            from cfgInt in Configr.Primitive(Fuzzr.Constant(42))
            from person in Fuzzr.One<NullablePerson>()
            select person;
        var result = fuzzr.Generate(1);
        Assert.Equal("mu", result.Name);
        Assert.Equal(42, result.Age);
    }

    [Fact]
    [DocOverloads]
    [DocContent("- `Primitive<T>(this FuzzrOf<T?> fuzzr)`:")]
    [DocContent("  Registers a global default fuzzr for nullable primitives `T?`, overriding all nullable values produced across generated objects.")]
    [DocExample(typeof(ConfigrPrimitive), nameof(GetNullableFuzzr))]
    [DocContent("  Replacing a nullable primitive generator does not impacts it's non-nullable counterpart.")]
    public void Nullable()
    {
        var (person, nullablePerson) = GetNullableFuzzr().Generate(1).PulseToQuickLog();
        Assert.Equal("cmu", person.Name);
        Assert.Equal(66, person.Age);
        Assert.Equal("ycqa", nullablePerson.Name);
        Assert.Equal(42, nullablePerson.Age);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(Person, NullablePerson)> GetNullableFuzzr()
    {
        return
        from cfgString in Configr.Primitive(Fuzzr.Constant<int?>(42))
        from person in Fuzzr.One<Person>()
        from nullablePerson in Fuzzr.One<NullablePerson>()
        select (person, nullablePerson);
        // Results in => 
        // ( Person { Name: "cmu", Age: 66 }, NullablePerson { Name: "ycqa", Age: 42 } )
    }

    [Fact]
    [DocContent("- `Fuzzr.Primitive(this FuzzrOf<string> fuzzr)`:")]
    [DocContent("  Registers a global default fuzzr for strings, overriding all string values produced across generated objects.")]
    [DocExample(typeof(ConfigrPrimitive), nameof(GetFuzzrString))]
    public void Strings()
    {
        var (person, address) = GetFuzzrString().Generate(42);
        Assert.Equal("FIXED", person.Name);
        Assert.Equal(67, person.Age);
        Assert.Equal("FIXED", address.Street);
        Assert.Equal("FIXED", address.City);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(Person, Address)> GetFuzzrString()
    {
        return
        from cfgString in Configr.Primitive(Fuzzr.Constant("FIXED"))
        from person in Fuzzr.One<Person>()
        from address in Fuzzr.One<Address>()
        select (person, address);
        // Results in => 
        // ( Person { Name: "FIXED", Age: 67 }, Address { Street: "FIXED", City: "FIXED" } )
    }
}
