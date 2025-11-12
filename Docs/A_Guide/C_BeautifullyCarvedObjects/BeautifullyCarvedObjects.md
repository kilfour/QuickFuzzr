# Beautifully Carved Objects
Now that we understand how composition works in QuickFuzzr, let's examine how this concept is applied to object generation.  
## From Fragments to Forms
At its heart, object generation in QuickFuzzr is still composition.
The main tool for this is `Fuzzr.One<T>()`, which tells QuickFuzzr to create a complete instance of type `T`.

When QuickFuzzr does this, it adheres to the following (adjustable) conventions:  
- Primitive properties are generated using their default `Fuzzr` equivalents.  
- Enumerations are filled using `Fuzzr.Enum<T>()`.  
- Object properties are generated where possible.  

Example:  
```csharp
public class Appointment
{
    public TimeSlot TimeSlot { get; set; } = default!;
}
```
```csharp
public class TimeSlot
{
    public DayOfWeek Day { get; set; }
    public int Time { get; set; }
}
```
```csharp
Fuzzr.One<Appointment>().Generate();
// Results in => { TimeSlot: { Day: Thursday, Time: 14 } }
```
## Where QuickFuzzr Draws the Line
- By default, only properties with public setters are auto-generated.  
- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.  
- Recursive object creation is off by default.  

**Note:** All of QuickFuzzrs defaults can be overridden using `Configr`.  
## Where `One` Is Not Enough
### Construction
Consider:  
```csharp
public record PersonRecord(string Name, int Age);
public class NullablePerson
{
    public string? Name { get; set; } = string.Empty;
    public int? Age { get; set; }
}
```
This record does not have a default constructor, so `Fuzzr.One<PersonRecord>()`
will throw an exception with the following message if used as is:  
```text
Cannot generate instance of PersonRecord.
Possible solutions:
• Add a parameterless constructor
• Register a custom constructor: Configr<PersonRecord>.Construct(...)
• Use explicit generation: from x in Fuzzr.Int() ... select new PersonRecord(x)
• Use the factory method overload: Fuzzr.One<T>(Func<T> constructor)
```
As you can see the error message hints at possible solutions,
so here are the concrete ones (ignoring the parameterless constructor suggestion) for our current case:
  
**Configr.Construct:** Best for reusable configurations.  
```csharp
var vowel = Fuzzr.OneOf('a', 'e', 'o', 'u', 'i');
from cfg in Configr<PersonRecord>.Construct(Fuzzr.String(vowel, 2, 10), Fuzzr.Int())
from person in Fuzzr.One<PersonRecord>() // <= 'One' now works, thanks to Configr
select person;
// Results in => { Name = "aaoaeuoa", Age = 76 }
```
**Explicit generation**: Most useful for creating (reusable) Fuzzrs of the type to generate.  
```csharp
from name in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
from age in Fuzzr.Int(-100, 0)
select new PersonRecord(name, age);
// Results in => { Name = "George", Age = -86 }
```
**Factory method:** Most straightforward for one-off cases.  
```csharp
from name in Fuzzr.Constant("Who")
from age in Fuzzr.OneOf(1, 2, 3)
from person in Fuzzr.One(() => new PersonRecord(name, age))
select person;
// Results in => { Name = "Who", Age = 3 }
```
### Property Access
This class uses *init only* properties, which are ignored by QuickFuzzr's default settings.  
```csharp
public class PrivatePerson
{
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
}
```
We can change that using `Configr`:  
```csharp
from enable in Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)
from person1 in Fuzzr.One<PrivatePerson>()
from disable in Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)
from person2 in Fuzzr.One<PrivatePerson>()
select (person1, person2);
// Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
```
This demonstrates how QuickFuzzr gives you fine-grained control over which properties get generated, 
allowing you to work with various access modifiers and C# patterns.  
  
Also if you `Configr` a property explicitly, QuickFuzzr assumes you know what you're doing and generates a value:  
```csharp
from name in Configr<PrivatePerson>.Property(a => a.Name,
    from cnt in Fuzzr.Counter("person") select $"Person number {cnt}.")
from age in Configr<PrivatePerson>.Property(a => a.Age, Fuzzr.Int(18, 81))
from person in Fuzzr.One<PrivatePerson>()
select person;
// Results in => { Name: "Person number 1.", Age: 35 }
```
### Filling Collections
There's a couple of ways we can go about accomplishing this.  

*For this class*:  
```csharp
public class PublicAgenda
{
    public List<Appointment> Appointments { get; set; } = [];
}
```
We can just `Configr` the property:  
```csharp
from appointments in Fuzzr.One<Appointment>().Many(1, 3)
from cfg in Configr<PublicAgenda>.Property(a => a.Appointments, appointments)
from agenda in Fuzzr.One<PublicAgenda>()
select agenda;
// Results in => 
//     { Appointments: [ 
//         { TimeSlot: { Day: Sunday, Time: 13 } }, 
//         { TimeSlot: { Day: Wednesday, Time: 17 } } ] 
//     }
```
And for this more realistic example:  
```csharp
public class Agenda
{
    private readonly List<Appointment> appointments = [];
    public IReadOnlyList<Appointment> Appointments => appointments;
    public void Add(Appointment appointment) => appointments.Add(appointment);
}
```
We can use :  
```csharp
from agenda in Fuzzr.One<Agenda>()
from appointments in Fuzzr.One<Appointment>().Apply(agenda.Add).Many(1, 3)
select agenda;
// Results in => 
//     { Appointments: [ 
//         { TimeSlot: { Day: Friday, Time: 52 } }, 
//         { TimeSlot: { Day: Saturday, Time: 8 } } ] 
//     }
```
