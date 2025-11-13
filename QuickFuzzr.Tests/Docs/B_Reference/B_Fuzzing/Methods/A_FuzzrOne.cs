using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileCodeHeader("Fuzzr.One<T>()")]
[DocColumn(Fuzzing.Columns.Description, "Creates a fuzzr that produces an instances of type `T`.")]
[DocContent(
@"Creates a fuzzr that produces complete instances of type `T` using QuickFuzzr's automatic construction rules: ")]
public class A_FuzzrOne
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<Person> Person_Example_Fuzzr()
    {
        return Fuzzr.One<Person>();
    }
    // Results in => { Name: "ddnegsn", Age: 18 }

    [Fact]
    [DocUsage]
    [DocExample(typeof(A_FuzzrOne), nameof(Person_Example_Fuzzr))]
    [DocContent(" - Uses `T`'s public parameterless constructor. Parameterized ctors aren't auto-filled.")]
    public void Person_Example()
    {
        var result = Person_Example_Fuzzr().Generate(42);
        Assert.Equal("ddnegsn", result.Name);
        Assert.Equal(18, result.Age);
    }

    [Fact]
    [DocContent("- Primitive properties are generated using their default `Fuzzr` equivalents.")]
    public void FuzzrOneFillsPrimitives()
    {
        var result = Fuzzr.One<TimeSlot>().Generate(42);
        Assert.Equal(14, result.Time);
    }

    [Fact]
    [DocContent("- Enumerations are filled using `Fuzzr.Enum<T>()`.")]
    public void FuzzrOneFillsEnums() =>
        Assert.Equal(DayOfWeek.Thursday, Fuzzr.One<TimeSlot>().Generate(42).Day);

    [Fact]
    [DocContent("- Object properties are generated where possible.")]
    public void FuzzrOneFillsObjectProperties()
    {
        var result = Fuzzr.One<Appointment>().Generate(42);
        Assert.Equal(DayOfWeek.Thursday, result.TimeSlot.Day);
        Assert.Equal(14, result.TimeSlot.Time);
    }

    [Fact]
    [DocContent("- By default, only properties with public setters are auto-generated.")]
    public void FuzzrOne_Only_Generates_For_Public_Setters()
    {
        var result = Fuzzr.One<PrivatePerson>().Generate(42);
        Assert.Equal(string.Empty, result.Name);
        Assert.Equal(0, result.Age);
    }

    [Fact]
    [DocContent("- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.")]
    public void FuzzrOne_No_Collections_Generated()
    {
        var result = Fuzzr.One<PublicAgenda>().Generate(42);
        Assert.Equal([], result.Appointments);
    }

    [Fact]
    [DocContent("- Recursive object creation is off by default.")]
    public void FuzzrOne_Recursion_Depth()
    {
        var result = Fuzzr.One<Folder>().Generate(42);
        Assert.Null(result.SubFolder);
    }

    [Fact]
    [DocContent("- Field generation is not supported.")]
    public void FuzzrOne_No_Fields()
    {
        var result = Fuzzr.One<PersonOutInTheFields>().Generate(42);
        Assert.Equal(string.Empty, result.Name);
        Assert.Equal(0, result.Age);
    }

    [Fact]
    [DocOverloads]
    [DocContent("- `Fuzzr.One<T>(Func<T> constructor)`:")]
    [DocContent("  Creates a fuzzr that produces instances of T by invoking the supplied factory on each generation.")]
    public void FuzzrOne_No_Default_Ctor_Construction_Options()
    {
        var fuzzr =
            from name in Fuzzr.Constant("Who")
            from age in Fuzzr.OneOf(1, 2, 3)
            from person in Fuzzr.One(() => new PersonRecord(name, age))
            select person;
        var result = fuzzr.Generate(42);
        Assert.Equal("Who", result.Name);
        Assert.Equal(3, result.Age);
    }

    [Fact]
    [DocExceptions]
    [DocException("ConstructionException", "When type T cannot be constructed due to missing default constructor.")]
    public void FuzzrOne_No_Default_Ctor_Throws()
    {
        var ex = Assert.Throws<ConstructionException>(() => Fuzzr.One<PersonRecord>().Generate());
        Assert.Equal(FuzzrOne_No_Default_Ctor_Throws_Message(), ex.Message);
    }

    private static string FuzzrOne_No_Default_Ctor_Throws_Message() =>
@"Cannot generate instance of PersonRecord.

Possible solutions:
• Add a parameterless constructor
• Register a custom constructor: Configr<PersonRecord>.Construct(...)
• Use explicit generation: from x in Fuzzr.Int() ... select new PersonRecord(x)
• Use the factory method overload: Fuzzr.One<T>(Func<T> constructor)
";


    [Fact]
    [DocException("InstantiationException", "When type T is an interface and cannot be instantiated.")]
    public void Generating_Abstract_Class_Shows_Helpfull_Exception()
    {
        var ex = Assert.Throws<InstantiationException>(() => Fuzzr.One<AbstractPerson>().Generate());
        Assert.Equal(Generating_Abstract_Class_Message(), ex.Message);
    }

    private static string Generating_Abstract_Class_Message() =>
@"Cannot generate an instance of the abstract class AbstractPerson.

Possible solution:
• Register one or more concrete subtype(s): Configr<AbstractPerson>.AsOneOf(...)
";

    [Fact]
    [DocContent("- `NullReferenceException`:")]
    [DocContent("  - When the factory method returns null.")]
    public void FactoryMethod_Returning_Null_Throws() // Check: Update message
    {
        var ex = Assert.Throws<NullReferenceException>(() => Fuzzr.One<Person>(() => null!).Generate());
        Assert.Equal(FactoryMethod_Returning_Null_Message(), ex.Message);
    }

    private static string FactoryMethod_Returning_Null_Message() =>
@"Object reference not set to an instance of an object.";

    [Fact]
    [DocContent("  - When the factory method is null.")]
    public void FactoryMethod_Is_Null_Throws() // Check: Update message
    {
        var ex = Assert.Throws<NullReferenceException>(() => Fuzzr.One<Person>(null!).Generate());
        Assert.Equal(FactoryMethod_Is_Null_Message(), ex.Message);
    }

    private static string FactoryMethod_Is_Null_Message() =>
@"Object reference not set to an instance of an object.";
}
