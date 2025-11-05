using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.C_BeautifullyCarvedObjects;

[DocFile]
[DocContent("Now that we understand how composition works in QuickFuzzr, let's examine how this concept is applied to object generation.")]
public class BeautifullyCarvedObjects
{
    [Fact]
    [DocHeader("From Fragments to Forms")]
    [DocContent(
@"At its heart, object generation in QuickFuzzr is still composition.
The main tool for this is `Fuzzr.One<T>()`, which tells QuickFuzzr to create a complete instance of type `T`.

When QuickFuzzr does this, it adheres to the following (adjustable) conventions:")]
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
    [DocContent("- Object properties are recursively generated where possible.")]
    public void FuzzrOneFillsObjectProperties()
    {
        var result = Fuzzr.One<Appointment>().Generate(42);
        Assert.Equal(DayOfWeek.Thursday, result.TimeSlot.Day);
        Assert.Equal(14, result.TimeSlot.Time);
    }

    [Fact]
    [DocContent("\nExample:")]
    [DocExample(typeof(Appointment))]
    [DocExample(typeof(TimeSlot))]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(SimpleObject_Example))]
    public void SimpleObject()
    {
        var result = SimpleObject_Example();
        Assert.Equal(DayOfWeek.Thursday, result.TimeSlot.Day);
        Assert.Equal(14, result.TimeSlot.Time);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("42")]
    private static Appointment SimpleObject_Example()
    {
        return Fuzzr.One<Appointment>().Generate(42);
        // Results in => { TimeSlot: { Day: Thursday, Time: 14 } }
    }

    [Fact]
    [DocHeader("Where QuickFuzzr Draws the Line")]
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
    [DocContent("\n**Note:** All of QuickFuzzrs defaults can be overridden using `Configr`.")]
    public void FuzzrOne_Recursion_Depth()
    {
        var result = Fuzzr.One<Folder>().Generate(42);
        Assert.Null(result.SubFolder);
    }

    [Fact]
    [DocHeader("Where `One` Is Not Enough")]
    [DocHeader("Construction", 1)]
    [DocContent("Consider:")]
    [DocExample(typeof(PersonRecord))]
    [DocContent(
@"This record does not have a default constructor, so `Fuzzr.One<PersonRecord>()`
will throw an exception with the following message if used as is:")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_No_Default_Ctor_Throws_Message), "text")]
    public void FuzzrOne_No_Default_Ctor_Throws()
    {
        var ex = Assert.Throws<ConstructionException>(() => Fuzzr.One<PersonRecord>().Generate());
        Assert.Equal(FuzzrOne_No_Default_Ctor_Throws_Message(), ex.Message);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    [CodeRemove("@\"")]
    [CodeRemove("\";")]
    private static string FuzzrOne_No_Default_Ctor_Throws_Message()
    {
        return
@"Cannot generate instance of PersonRecord.

Possible solutions:
• Add a parameterless constructor
• Register a custom constructor: Configr<PersonRecord>.Construct(...)
• Use explicit generation: from x in Fuzzr.Int() ... select new PersonRecord(x)
• Use the factory method overload: Fuzzr.One<T>(Func<T> constructor)
";
    }

    [Fact]
    [DocContent(
@"As you can see the error message hints at possible solutions,
so here are the concrete ones (ignoring the parameterless constructor suggestion) for our current case:
")]
    [DocContent("**Configr.Construct:** Best for reusable configurations.")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_No_Default_Ctor_Register_Custom_Constructor))]
    [DocContent("**Explicit generation**: Most useful for creating (reusable) Fuzzrs of the type to generate.")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_No_Default_Ctor_Explicit_Generation))]
    [DocContent("**Factory method:** Most straightforward for one-off cases.")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_No_Default_Ctor_Factory_Method))]
    public void FuzzrOne_No_Default_Ctor_Construction_Options()
    {
        var result = FuzzrOne_No_Default_Ctor_Register_Custom_Constructor().Generate(42);
        Assert.Equal("aaoaeuoa", result.Name);
        Assert.Equal(76, result.Age);
        result = FuzzrOne_No_Default_Ctor_Explicit_Generation().Generate(42);
        Assert.Equal("George", result.Name);
        Assert.Equal(-86, result.Age);
        result = FuzzrOne_No_Default_Ctor_Factory_Method().Generate(42);
        Assert.Equal("Who", result.Name);
        Assert.Equal(3, result.Age);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<PersonRecord> FuzzrOne_No_Default_Ctor_Register_Custom_Constructor()
    {
        var vowel = Fuzzr.OneOf('a', 'e', 'o', 'u', 'i');
        return
        from cfg in Configr<PersonRecord>.Construct(Fuzzr.String(vowel, 2, 10), Fuzzr.Int())
        from person in Fuzzr.One<PersonRecord>() // <= 'One' now works, thanks to Configr
        select person;
        // Results in => { Name = "aaoaeuoa", Age = 76 }
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<PersonRecord> FuzzrOne_No_Default_Ctor_Explicit_Generation()
    {
        return
        from name in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
        from age in Fuzzr.Int(-100, 0)
        select new PersonRecord(name, age);
        // Results in => { Name = "George", Age = -86 }
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<PersonRecord> FuzzrOne_No_Default_Ctor_Factory_Method()
    {
        return
        from name in Fuzzr.Constant("Who")
        from age in Fuzzr.OneOf(1, 2, 3)
        from person in Fuzzr.One(() => new PersonRecord(name, age))
        select person;
        // Results in => { Name = "Who", Age = 3 }
    }

    [Fact]
    [DocHeader("Property Access", 1)]
    [DocContent("This class uses *init only* properties, which are ignored by QuickFuzzr's default settings.")]
    [DocExample(typeof(PrivatePerson))]
    [DocContent("We can change that using `Configr`:")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_Property_Access_Fuzzr))]
    [DocContent(
@"This demonstrates how QuickFuzzr gives you fine-grained control over which properties get generated, 
allowing you to work with various access modifiers and C# patterns.  
")]
    public void FuzzrOne_Property_Access()
    {
        var (person1, person2) = FuzzrOne_Property_Access_Fuzzr().Generate(1234);
        Assert.Equal("xiyi", person1.Name);
        Assert.Equal(94, person1.Age);
        Assert.Equal("", person2.Name);
        Assert.Equal(0, person2.Age);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<(PrivatePerson person1, PrivatePerson person2)> FuzzrOne_Property_Access_Fuzzr()
    {
        return
        from enable in Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)
        from person1 in Fuzzr.One<PrivatePerson>()
        from disable in Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)
        from person2 in Fuzzr.One<PrivatePerson>()
        select (person1, person2);
        // Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
    }

    [Fact]
    [DocContent("Also if you `Configr` a property explicitly QuickFuzzr assumes you know what you're doing and generates a value:")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_Property_Acces_Override_Fuzzr))]
    public void FuzzrOne_Property_Acces_Override()
    {
        var result = FuzzrOne_Property_Acces_Override_Fuzzr().Generate(47);
        Assert.Equal("Person number 1.", result.Name);
        Assert.Equal(35, result.Age);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private FuzzrOf<PrivatePerson> FuzzrOne_Property_Acces_Override_Fuzzr()
    {
        return
        from name in Configr<PrivatePerson>.Property(a => a.Name,
            from cnt in Fuzzr.Counter("person") select $"Person number {cnt}.")
        from age in Configr<PrivatePerson>.Property(a => a.Age, Fuzzr.Int(18, 81))
        from person in Fuzzr.One<PrivatePerson>()
        select person;
        // Results in => { Name: "Person number 1.", Age: 35 }
    }

    [Fact]
    [DocHeader("Filling Collections", 1)]
    [DocContent(
@"There's a couple of ways we can go about accomplishing this.  

*For this class*:")]
    [DocExample(typeof(PublicAgenda))]
    [DocContent("We can just `Configr` the property:")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_Collections_Fuzzr))]
    public void FuzzrOne_Collections()
    {
        var result = FuzzrOne_Collections_Fuzzr().Generate(42);
        Assert.Equal(DayOfWeek.Sunday, result.Appointments[0].TimeSlot.Day);
        Assert.Equal(13, result.Appointments[0].TimeSlot.Time);
        Assert.Equal(DayOfWeek.Wednesday, result.Appointments[1].TimeSlot.Day);
        Assert.Equal(17, result.Appointments[1].TimeSlot.Time);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<PublicAgenda> FuzzrOne_Collections_Fuzzr()
    {
        return
        from appointments in Fuzzr.One<Appointment>().Many(1, 3)
        from cfg in Configr<PublicAgenda>.Property(a => a.Appointments, appointments)
        from agenda in Fuzzr.One<PublicAgenda>()
        select agenda;
        // Results in => 
        //     { Appointments: [ 
        //         { TimeSlot: { Day: Sunday, Time: 13 } }, 
        //         { TimeSlot: { Day: Wednesday, Time: 17 } } ] 
        //     }
    }

    [Fact]
    [DocContent("And for this more realistic example:")]
    [DocExample(typeof(Agenda))]
    [DocContent("We can use :")]
    [DocExample(typeof(BeautifullyCarvedObjects), nameof(FuzzrOne_Collections_Method_Fuzzr))]
    public void FuzzrOne_Collections_Method()
    {
        var result = FuzzrOne_Collections_Method_Fuzzr().Generate(44);
        Assert.Equal(DayOfWeek.Friday, result.Appointments[0].TimeSlot.Day);
        Assert.Equal(52, result.Appointments[0].TimeSlot.Time);
        Assert.Equal(DayOfWeek.Saturday, result.Appointments[1].TimeSlot.Day);
        Assert.Equal(8, result.Appointments[1].TimeSlot.Time);

    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Agenda> FuzzrOne_Collections_Method_Fuzzr()
    {
        return
        from agenda in Fuzzr.One<Agenda>()
        from appointments in Fuzzr.One<Appointment>().Apply(agenda.Add).Many(1, 3)
        select agenda;
        // Results in => 
        //     { Appointments: [ 
        //         { TimeSlot: { Day: Friday, Time: 52 } }, 
        //         { TimeSlot: { Day: Saturday, Time: 8 } } ] 
        //     }
    }
}