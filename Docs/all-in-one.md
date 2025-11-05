## Your First Fuzzr
Starting simple. Suppose you have this class:  
```csharp
public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
```
You can generate a fully randomized instance like so:  
```csharp
Fuzzr.One<Person>().Generate();
```
**Output:**  
```text
Person { Name = "ddnegsm", Age = 18 }
```
And that's it, ... no configuration required.  
QuickFuzzr walks the `Person` type, detects its properties,
and fills them in using the default generators.  
### Your First Customization
QuickFuzzr is primarily designed to generate inputs for **fuzz-testing**, 
 so the default settings are mainly aimed towards that goal.  
 It is relatively easy however to *customize* the generation.  
 Let's for instance generate a *real* name and ensure our `Person` is eligible to vote:  
```csharp
var personFuzzr =
    from firstname in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
    from lastname in Fuzzr.OneOf("Lennon", "McCartney", "Harrison", "Star")
    from age in Fuzzr.Int(18, 80)
    select new Person { Name = $"{firstname} {lastname}", Age = age };
personFuzzr.Generate();
```
**Output:**  
```text
Person { Name = "George Lennon", Age = 25 }
```
### Composable Fuzzrs
Above example already shows how QuickFuzzr leverages the composability of `LINQ` to good effect,
but let's drive that point home a bit more.  
Consider this `Employee` class derived from `Person`:  
```csharp
public class Employee : Person
{
    public string Email { get; set; } = string.Empty;
    public string SocialSecurityNumber { get; set; } = string.Empty;
}
```
Then, when defining a Fuzzr like so:

  
**Social Security Number**  
```csharp
var ssnFuzzr =
    from a in Fuzzr.Int(100, 999)
    from b in Fuzzr.Int(10, 99)
    from c in Fuzzr.Int(1000, 9999)
    select $"{a}-{b}-{c}";
```
**Employee**  
```csharp
var employeeFuzzr =
    from firstname in Fuzzr.OneOf(firstnames)
    from lastname in Fuzzr.OneOf(lastnames)
    from age in Fuzzr.Int(18, 80)
    from ssn in ssnFuzzr
    from email_provider in Fuzzr.OneOf(emailProviders)
    select
        new Employee
        {
            Name = $"{firstname} {lastname}",
            Age = age,
            Email = $"{firstname}.{lastname}@{email_provider}",
            SocialSecurityNumber = ssn
        };
```
**Output:**  
```
{
    Email: "John McCartney@company.com",
    SocialSecurityNumber: "761-65-2228",
    Name: "John McCartney",
    Age: 69
}
```
In this example the lists used by `OneOf` are declared outside of the generator.
I just used `string[]`'s but the data could easily be loaded from a file for instance.  
### Please, Sir, I Want Some More.
For MORE!, ..., oh well then.  
Just use:  
```csharp
employeeFuzzr.Many(3).Generate()
```
**Output:**  
```
[
    {
        Email: "George.Harrison@mailings.org",
        SocialSecurityNumber: "953-16-1093",
        Name: "George Harrison",
        Age: 50
    },
    {
        Email: "George.McCartney@mailings.org",
        SocialSecurityNumber: "736-82-8923",
        Name: "George McCartney",
        Age: 48
    },
    {
        Email: "Ringo.Harrison@mailings.org",
        SocialSecurityNumber: "347-87-4164",
        Name: "Ringo Harrison",
        Age: 40
    }
]
```
### The Strand Union Workhouse
Another *derivation*:  
```csharp
public class HousedEmployee : Employee
{
    public Address Address { get; set; } = new Address();
}
```
Now we could use an `addressFuzzr` and add another line to the `select` clause of the `LINQ` expression,
but let's introduce the final piece of the QuickFuzzr puzzle: `Configr`,
and rewrite it like so:

  
**Address**  
```csharp
var addressConfigr =
    from street in Configr<Address>.Property(a => a.Street, Fuzzr.OneOf(streets))
    from city in Configr<Address>.Property(a => a.City, Fuzzr.OneOf(cities))
    select Intent.Fixed;
```
**Info**  
> This is a helper `record` in order to correlate name and email.  
```csharp
var infoFuzzr =
    from firstname in Fuzzr.OneOf(firstnames)
    from lastname in Fuzzr.OneOf(lastnames)
    from email_provider in Fuzzr.OneOf(emailProviders)
    select new Info(
        $"{firstname} {lastname}",
        $"{firstname}.{lastname}@{email_provider}");
```
**Person**  
```csharp
var employeeConfigr =
    from _ in Configr<Person>.With(infoFuzzr, info =>
        from name in Configr<Person>.Property(a => a.Name, info.Name)
        from email in Configr<Employee>.Property(a => a.Email, info.Email)
        select Intent.Fixed)
    from age in Configr<Person>.Property(a => a.Age, Fuzzr.Int(18, 80))
    from ssn in Configr<Employee>.Property(a => a.SocialSecurityNumber, ssnFuzzr)
    select Intent.Fixed;
```
**All Together Now**  
```csharp
var peopleFuzzr =
    from addressCfg in addressConfigr
    from employeeCfg in employeeConfigr
    from housedEmployee in Fuzzr.One<HousedEmployee>()
    select housedEmployee;
```
**Output:**  
```
{
    Address: {
        Street: "Victoria Street",
        City: "Manchester"
    },
    Email: "George.Lennon@company.com",
    SocialSecurityNumber: "336-74-5615",
    Name: "George Lennon",
    Age: 28
}
```
Ok, calling this chapter *"Your First Fuzzr"*, might have been a bit optimistic.
Because that is some dense `LINQ`-ing right there.  
But here are some counter arguments.  

**1. You can always just**: Use `Fuzzr.One<HousedEmployee>()`, resulting in, for instance:  
```text
{
    Address: {
        Street: "u",
        City: "xjgw"
    },
    Email: "dqasmq",
    SocialSecurityNumber: "ggxgtn",
    Name: "t",
    Age: 30
}
```
**2. Reusability**: Once you have defined a `LINQ` chain like the one above, you can do more than one thing with it.

**Generate just addresses (or `Person`, `Employee`, etc.):**
  
```csharp
var fuzzr =
    from cfg in peopleFuzzr
    from address in Fuzzr.One<Address>()
    select address;
fuzzr.Many(3).Generate();
```
**Output:**  
```
[
    {
        Street: "High Street",
        City: "Bristol"
    },
    {
        Street: "Victoria Street",
        City: "London"
    },
    {
        Street: "Kings Road",
        City: "Liverpool"
    }
]
```
**3. Composability**: Or configure and reconfigure on the fly:  
```csharp
var fuzzr =
    from cfg in peopleFuzzr
        // Generate a HousedEmployee according to current configuration
    from normal in Fuzzr.One<HousedEmployee>()
        // Override city for this HousedEmployee
    from london in Configr<Address>.Property(a => a.City, "London")
    from londoner in Fuzzr.One<HousedEmployee>()
        // Restore city config and override Age
    from city in Configr<Address>.Property(a => a.City, Fuzzr.OneOf(cities))
    from age in Configr<Person>.Property(a => a.Age, Fuzzr.Int(8, 17))
    from underaged in Fuzzr.One<HousedEmployee>()
        // Return a Tuple (or, ..., a Thruple ?) of HousedEmployees
    select (normal, londoner, underaged);
fuzzr.Generate();
```
**Output:**  
```
(
    {
        Address: {
            Street: "Station Road",
            City: "Bristol"
        },
        Email: "Paul.McCartney@mailings.org",
        SocialSecurityNumber: "621-54-5020",
        Name: "Paul McCartney",
        Age: 27
    },
    {
        Address: {
            Street: "Victoria Street",
            City: "London"
        },
        Email: "George.Harrison@freemail.net",
        SocialSecurityNumber: "535-80-4278",
        Name: "George Harrison",
        Age: 34
    },
    {
        Address: {
            Street: "Kings Road",
            City: "Manchester"
        },
        Email: "Paul.Star@company.com",
        SocialSecurityNumber: "428-67-7239",
        Name: "Paul Star",
        Age: 11
    }
)
```
As you can see this reuses the previous Fuzzr, generates a *normal* `HousedEmployee`,
another one that is guaranteed to live in London, and finally an underaged one.  
## On Composition
In the previous chapter, the examples kind of went from zero to sixty in under three, so let's step back a bit and have a look under the hood.  
### Fuzzr
The basic building block of QuickFuzzr is a `FuzzrOf<T>`. This is in essence a function that returns a random value of type `T`.  
As you have seen, QuickFuzzr is *`LINQ`-enabled*, meaning these building blocks can be chained together using `LINQ`.  
The first way to create `FuzzrOf<T>` instances is by calling the methods on the static factory class `Fuzzr`.   

Let's look at the *social security number Fuzzr* from before, for instance:  
```csharp
var ssnFuzzr =
    from a in Fuzzr.Int(100, 999)
    from b in Fuzzr.Int(10, 99)
    from c in Fuzzr.Int(1000, 9999)
    select $"{a}-{b}-{c}";
```
Here the `Fuzzr.Int(...)` calls produce a `FuzzrOf<int>`, so that in the left side of the `LINQ` statements, the range variables are of type `int`.  
The `select` then combines those using string interpolation, meaning that the whole thing is also a `Fuzzr` but now of type `FuzzrOf<string>`.  


*Sidenote:* There's usually more than one way to get things done in QuickFuzzr.
As an example of that, here is another social security generator, this time using `char`'s and `string`'s:  
```csharp
var digit = Fuzzr.Char('0', '9');
var ssnFuzzr =
    from a in Fuzzr.String(digit, 3)
    from b in Fuzzr.String(digit, 2)
    from c in Fuzzr.String(digit, 4)
    select $"{a}-{b}-{c}";
// Results in => "114-26-1622"
```
### Configr
The second way of getting a hold of `FuzzrOf<T>` building blocks is by calling the methods on the static factory class `Configr`.
These however return a *special* type: a `FuzzrOf<Intent>`.  
This essentially means they do not return a value. In functional speak `Intent` is QuickFuzzr's `Unit` type.
The range variable in the `LINQ` chain is always ignored.

The calls to `Configr` in effect do not generate values, they exist to **influence generation** down the line.  
  
The fact that they are a form of `FuzzrOf<T>` however, allows us to sprinkle them into the `LINQ` chain and configure generation on the fly.  
```csharp
var personFuzzr =
    from _ in Configr<Person>.Property(p => p.Age, Fuzzr.Int(666, 777))
    from person in Fuzzr.One<Person>()
    select person;
// Results in => { Name: "wsusbadepw", Age: 720 }
```
### Extension Methods
Lastly, there are some extension methods defined for `FuzzrOf<T>`. I think the most obvious one is `.Many(...)`.  
These extension methods wrap the `FuzzrOf<T>` they are attached to and again, influence generation.  
In the case of `.Many(...)`, it runs the original `Fuzzr` as many times as specified in the arguments and returns the resulting values as an `IEnumerable<T>`.  

Example:  
```csharp
Fuzzr.Int().Many(3);
// Results in => [ 67, 14, 13 ]
```
#### A Word of Caution
When using the `FuzzrOf<T>` extension methods, scope is important.    

For instance the following might produce an, at first glance, surprising result:  
```csharp
return
from name in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
from age in Fuzzr.Int(18, 99)
from people in Fuzzr.One(() => new PersonRecord(name, age)).Many(2)
select people;
// Results in => [ { Name: "George", Age: 69 }, { Name: "George", Age: 69 } ]
```
Looking closer however, it becomes clear this is correct behaviour according to the `LINQ` rules.  
The `name` and  `age` range variables are *captured* from outside of the scope of the `FuzzrOf<PersonRecord>`,
so calling `.Many(2)` does not cause them to be regenerated.  

A corrected version of this Fuzzr would look like this:  
```csharp
return
(from name in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
 from age in Fuzzr.Int(18, 99)
 from person in Fuzzr.One(() => new PersonRecord(name, age))
 select person)
    .Many(2);
// Results in => [ { Name: "Paul", Age: 88 }, { Name: "Ringo", Age: 73 } ]
```
## Beautifully Carved Objects
Now that we understand how composition works in QuickFuzzr, let's examine how this concept is applied to object generation.  
### From Fragments to Forms
At its heart, object generation in QuickFuzzr is still composition.
The main tool for this is `Fuzzr.One<T>()`, which tells QuickFuzzr to create a complete instance of type `T`.

When QuickFuzzr does this, it adheres to the following (adjustable) conventions:  
- Primitive properties are generated using their default `Fuzzr` equivalents.  
- Enumerations are filled using `Fuzzr.Enum<T>()`.  
- Object properties are recursively generated where possible.  

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
### Where QuickFuzzr Draws the Line
- By default, only properties with public setters are auto-generated.  
- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.  
- Recursive object creation is off by default.  

**Note:** All of QuickFuzzrs defaults can be overridden using `Configr`.  
### Where `One` Is Not Enough
#### Construction
Consider:  
```csharp
public record PersonRecord(string Name, int Age);
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
#### Property Access
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
// Results in => ( { Name: "whxi", Age: 94 }, { Name: "", Age: 0 } )
```
This demonstrates how QuickFuzzr gives you fine-grained control over which properties get generated, 
allowing you to work with various access modifiers and C# patterns.  
  
Also if you `Configr` a property explicitly QuickFuzzr assumes you know what you're doing and generates a value:  
```csharp
from name in Configr<PrivatePerson>.Property(a => a.Name,
    from cnt in Fuzzr.Counter("person") select $"Person number {cnt}.")
from age in Configr<PrivatePerson>.Property(a => a.Age, Fuzzr.Int(18, 81))
from person in Fuzzr.One<PrivatePerson>()
select person;
// Results in => { Name: "Person number 1.", Age: 35 }
```
#### Filling Collections
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
## Through the Looking Glass
In the previous chapter we hinted at recursion and depth-control.  

So, let's dive deeper.  

  
Imagine:  
```csharp
public class Folder
{
    public string Name { get; set; } = default!;
    public Folder? SubFolder { get; set; }
}
```
Calling `Fuzzr.One<Folder>().Generate()` results in:  
```text
{  Name: "ljcuu", SubFolder: null }
```
You can however influence how deep the rabbit hole goes,
by adding call to `Configr<Folder>.Depth(min, max)`:  
```csharp
return
from name in Configr<Folder>.Property(a => a.Name,
    from cnt in Fuzzr.Counter("folder") select $"Folder-{cnt}")
from folderDepth in Configr<Folder>.Depth(2, 5)
from folder in Fuzzr.One<Folder>()
select folder;
```
**Output:**  
```text
{
    Name: "Folder-1",
    SubFolder: {
        Name: "Folder-2",
        SubFolder: {
            Name: "Folder-3",
            SubFolder: null
        }
    }
}
```
Neat.  
But we can still go *one step beyond*.  

Consider this small model:
  
```csharp
public abstract class FileSystemEntry
{
    public string Name { get; set; } = default!;
}
```
```csharp
public class FileEntry : FileSystemEntry { }
```
```csharp
public class FolderEntry : FileSystemEntry
{
    public List<FileEntry> Files { get; set; } = [];
    public List<FolderEntry> Folders { get; set; } = [];
}
```
A bit complicated to fuzz, but let's have a go:
  
**Helper Function for Name Properties**  
```csharp
private static FuzzrOf<Intent> GetName<T>(string prefix) where T : FileSystemEntry
{
    return
        from name in Configr<T>.Property(a => a.Name,
            from cnt in Fuzzr.Counter(prefix) select $"{prefix}-{cnt}")
        select Intent.Fixed;
}
```
**Name Properties Configuration**  
```csharp
var nameCfg =
    from filename in GetName<FileEntry>("File")
    from foldername in GetName<FolderEntry>("Folder")
    select Intent.Fixed;
```
**Folder List Properties Configuration**  
```csharp
var folderCfg =
    from files in Configr<FolderEntry>.Property(a => a.Files,
        from fs in Fuzzr.One<FileEntry>().Many(1, 3) select fs.ToList())
    from folders in Configr<FolderEntry>.Property(a => a.Folders,
        from fs in Fuzzr.One<FolderEntry>().Many(1, 3) select fs.ToList())
    select Intent.Fixed;
```
**Combined Fuzzr**  
```csharp
var fuzzr =
    from names in nameCfg
    from lists in folderCfg
    from folderDepth in Configr<FolderEntry>.Depth(1, 3)
    from inheritance in Configr<FileSystemEntry>.AsOneOf(typeof(FolderEntry), typeof(FileEntry))
    from entry in Fuzzr.One<FolderEntry>()
    select entry;
```
**Output:**  
```text
{
    Files: [ { Name: "File-1" }, { Name: "File-2" } ],
    Folders: [
        {
            Files: [ { Name: "File-3" } ],
            Folders: [
                {
                    Files: [ { Name: "File-4" } ],
                    Folders: [ ],
                    Name: "Folder-1"
                },
                {
                    Files: [ { Name: "File-5" } ],
                    Folders: [ ],
                    Name: "Folder-2"
                }
            ],
            Name: "Folder-3"
        },
        {
            Files: [ { Name: "File-6" } ],
            Folders: [
                {
                    Files: [ { Name: "File-7" } ],
                    Folders: [ ],
                    Name: "Folder-4"
                }
            ],
            Name: "Folder-5"
        }
    ],
    Name: "Folder-6"
}
```
At this point QuickFuzzr has *type walked* an object graph that contains itself, stopped at a reasonable depth,
and made sense of collections nested inside collections, with controlled recursion.

Each type involved carries its own depth constraint, and every recursive property or list of child elements
simply burns through that budget one level at a time.
When the counter hits zero, the generator yields null (or an empty list), and the story ends right there.

It also means you can mix these with inheritance and collection combinators.  
And depth is local, not global: one deep branch does not force all others to go equally deep.  
## The Final Showcase
So, ..., if we have a domain that contains GenericIdentities, ValueObjects, Aggregates, etc.,
how *exactly* do we handle that in practice?

This example uses the [HorsesForCourses](https://github.com/kilfour/HorsesForCourses) example domain.  
*Note:* `Admin` is a system user authorized for mutations, defined elsewhere in the domain model.  
#### Building Blocks
**Generic Identity**
- Generate a stable, per-type increasing id.  
```csharp
private static FuzzrOf<int> GenericId<T>()
    where T : DomainEntity<T>
{
    return
    from id in Fuzzr.Counter($"{typeof(T).Name.ToLower()}-id")
    from _ in Configr<T>.Property(a => a.Id, Id<T>.From(id))
    select id;
}
```
**Name and Email**
- Build a consistent Info(name, email) pair.
- Depends on external *DataLists*.  
```csharp
// Name
from isMale in Fuzzr.Bool()
let firstNames = isMale ? DataLists.MaleFirstNames : DataLists.FemaleFirstNames
from firstName in Fuzzr.OneOf(firstNames)
from lastName in Fuzzr.OneOf(DataLists.LastNames)
let name = $"{firstName} {lastName}"
// Email
from emailProvider in Fuzzr.OneOf(DataLists.EmailProviders)
from domain in Fuzzr.OneOf(DataLists.TopLevelDomains)
let email = $"{firstName}.{lastName}@{emailProvider}.{domain}".ToLower()
select new Info(name, email);
```
**Coach**
- Generates a complete `Coach` aggregate, linking skills and personal info.
- Requires the GenericId and InfoFuzzr from earlier and a `string[]` of skills.  
```csharp
from coachId in GenericId<Coach>()
from info in InfoFuzzr()
from skills in Fuzzr.OneOf(SkillPool).Unique(coachId).Many(3, 5)
from coach in Fuzzr.One(() => Coach.Create(Admin, info.Name, info.Email))
    .Apply(a => a.UpdateSkills(Admin, skills))
select coach;
```
**Period**
- Generates a valid course period within 2025.  
```csharp
from start in Fuzzr.DateOnly(1.January(2025), 31.December(2025))
from length in Fuzzr.Int(0, 120)
from end in Fuzzr.DateOnly(start, start.AddDays(length))
select (start, end);
```
**Timeslot**
- Produce a single (day, start, end) slot with sane hours.  
```csharp
from start in Fuzzr.Int(9, 17)
from end in Fuzzr.Int(start + 1, 18)
from day in Fuzzr.Enum<CourseDay>().Unique($"day-{key}")
select (day, start, end);
```
**Course**
- Construct a Course and bring it to a confirmed, assignable state.
- Requires the PeriodFuzzr and TimeslotFuzzrFor, the `string[]` of skills and a pool of coaches so one can potentially be assigned.  
```csharp
from courseId in GenericId<Course>()
    // Basic Info
from courseTitle in Fuzzr.OneOf(CourseTitles)
from period in PeriodFuzzr()
    // Course Construction
from course in Fuzzr.One(() => Course.Create(Admin, courseTitle, period.Start, period.End))
    // Values for Methods 
from requiredSkills in Fuzzr.OneOf(SkillPool).Many(1)
from timeslots in TimeslotFuzzrFor(courseId).Many(1, 3)
    // Calling Entity Methods
let _1 = course.UpdateRequiredSkills(Admin, requiredSkills)
let _2 = course.UpdateTimeSlots(Admin, [.. timeslots], a => a)
let _3 = course.Confirm(Admin)
from coachToAssign in Fuzzr.OneOfOrDefault(
    coaches.Where(a => a.IsSuitableFor(course) && a.IsAvailableFor(course)))
    // Assign a Coach if possible
select coachToAssign == null ? course : course.AssignCoach(Admin, coachToAssign);
```
**All Together Now**  
```csharp
from coaches in coachFuzzr.Many(3)
from course in CourseFuzzr(coaches)
select (course, coaches);
```
**Output:**  
```text
(
    {
        Name: "Advanced C# Patterns",
        Period: {
            Start: 26.November(2025),
            End: 7.January(2026)
        },
        TimeSlots: [
            { Day: Friday, Start: 11, End: 15 }
        ],
        RequiredSkills: [
            "C#"
        ],
        IsConfirmed: true,
        AssignedCoach: {
            Name: "Earl Owens",
            Email: "earl.owens@hotmail.org",
            Skills: [
                "JavaScript",
                "UnitTesting",
                "C#"
            ],
            AssignedCourses: [
                <cycle => Course.Id: 1>
            ],
            Id: 1,
            IsTransient: false
        },
        Id: 1,
        IsTransient: false
    },
    [
        {
            Name: "Earl Owens",
            Email: "earl.owens@hotmail.org",
            Skills: [
                "JavaScript",
                "UnitTesting",
                "C#"
            ],
            AssignedCourses: [
                {
                    Name: "Advanced C# Patterns",
                    Period: {
                        Start: 26.November(2025),
                        End: 7.January(2026)
                    },
                    TimeSlots: [
                        { Day: Friday, Start: 11, End: 15 }
                    ],
                    RequiredSkills: [
                        "C#"
                    ],
                    IsConfirmed: true,
                    AssignedCoach: <cycle => Coach.Id: 1>,
                    Id: 1,
                    IsTransient: false
                }
            ],
            Id: 1,
            IsTransient: false
        },
        {
            Name: "Leon Mccoy",
            Email: "leon.mccoy@hotmail.io",
            Skills: [
                "ASP.NET",
                "UnitTesting",
                "TDD"
            ],
            AssignedCourses: [ ],
            Id: 2,
            IsTransient: false
        },
        {
            Name: "Emily Figueroa",
            Email: "emily.figueroa@outlook.com",
            Skills: [
                "React",
                "UnitTesting",
                "TDD",
                "CI/CD"
            ],
            AssignedCourses: [ ],
            Id: 3,
            IsTransient: false
        }
    ]
)
```
#### Looking Back
This showcase demonstrates how QuickFuzzr can:
- Generate fully interconnected domain aggregates with bidirectional relationships
- Respect domain rules and business logic
- Handle identity generation and uniqueness constraints  
- Compose complex objects from simple building blocks
- Work with real-world domain patterns (entities, value objects, aggregates)  
## Reference
### Primitive Fuzzrs
#### Booleans
Use `Fuzzr.Bool()`.  
- The default generator generates True or False.  
- Can be made to return `bool?` using the `.Nullable()` combinator.  
- `bool` is automatically detected and generated for object properties.  
- `bool?` is automatically detected and generated for object properties.  
#### Bytes
Use `Fuzzr.Byte()`.  
- The default generator produces a `byte` in the full range (`0`-`255`).  
- The overload `Fuzzr.Byte(int min, int max)` generates a value greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- Throws an `ArgumentOutOfRangeException` when `min` < `byte.MinValue` (i.e. `< 0`).  
- Throws an `ArgumentOutOfRangeException` when `max` > `byte.MaxValue` (i.e. `> 255`).  
- When `min == max`, the generator always returns that exact value.  
- Boundary coverage: over time, values at both ends of the interval should appear.  
- Can be made to return `byte?` using the `.Nullable()` combinator.  
- `byte` is automatically detected and generated for object properties.  
- `byte?` is automatically detected and generated for object properties.  
#### Chars
Use `Fuzzr.Char()`.  
- The overload `Fuzzr.Char(char min, char max)` generates a char greater than or equal to `min` and less than or equal to `maxs`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator always generates a char between lower case 'a' and lower case 'z'.  
- Can be made to return `char?` using the `.Nullable()` combinator.  
- `char` is automatically detected and generated for object properties.  
- `char?` is automatically detected and generated for object properties.  
#### DateOnlys
Use `Fuzzr.DateOnly()`.  
- The overload `Fuzzr.DateOnly(DateOnly min, DateOnly max)` generates a DateOnly greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = new DateOnly(1970, 1, 1), max = new DateOnly(2020, 12, 31)).  
- Can be made to return `DateOnly?` using the `.Nullable()` combinator.  
- `DateOnly` is automatically detected and generated for object properties.  
- `DateOnly?` is automatically detected and generated for object properties.  
#### DateTimes
Use `Fuzzr.DateTime()`.  
- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a DateTime greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).  
- Can be made to return `DateTime?` using the `.Nullable()` combinator.  
- `DateTime` is automatically detected and generated for object properties.  
- `DateTime?` is automatically detected and generated for object properties.  
#### Decimals
Use `Fuzzr.Decimal()`.  
- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `decimal?` using the `.Nullable()` combinator.  
- `decimal` is automatically detected and generated for object properties.  
- `decimal?` is automatically detected and generated for object properties.  
#### Doubles
Use `Fuzzr.Double()`.  
- The overload `Fuzzr.Double(double min, double max)` generates a double greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `double?` using the `.Nullable()` combinator.  
- `double` is automatically detected and generated for object properties.  
- `double?` is automatically detected and generated for object properties.  
#### Enums
Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.  
> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.  
- The default generator just picks a random value from all enumeration values.  
- An Enumeration is automatically detected and generated for object properties.  
- A nullable enumeration is automatically detected and generated for object properties.  
- Passing in a non Enum type for T throws an ArgumentException.  
#### Floats
Use `Fuzzr.Float()`.  
- The overload `Fuzzr.Float(float min, float max)` generates a float greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `float?` using the `.Nullable()` combinator.  
- `float` is automatically detected and generated for object properties.  
- `float?` is automatically detected and generated for object properties.  
#### Guids
Use `Fuzzr.Guid()`. *There is no overload.*  
- The default generator never generates Guid.Empty.  
- `Fuzzr.Guid()` is deterministic when seeded.  
- Can be made to return `Guid?` using the `.Nullable()` combinator.  
- `Guid` is automatically detected and generated for object properties.  
- `Guid?` is automatically detected and generated for object properties.  
#### Halfs
Use `Fuzzr.Half()`.  
- The overload Fuzzr.Half(Half min, Half max) generates a half-precision floating-point number greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = (Half)1, max = (Half)100).  
- Can be made to return `Half?` using the `.Nullable()` combinator.  
- `Half` is automatically detected and generated for object properties.  
- `Half?` is automatically detected and generated for object properties.  
#### Ints
Use `Fuzzr.Int()`.  
- The overload `Fuzzr.Int(int min, int max)` generates an int greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `int?` using the `.Nullable()` combinator.  
- `int` is automatically detected and generated for object properties.  
- `int?` is automatically detected and generated for object properties.  
#### Longs
Use `Fuzzr.Long()`.  
- The overload `Fuzzr.Long(long min, long max)` generates a long greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `long?` using the `.Nullable()` combinator.  
- `long` is automatically detected and generated for object properties.  
- `long?` is automatically detected and generated for object properties.  
#### Shorts
Use `Fuzzr.Short()`.  
- The overload `Fuzzr.Short(short min, short max)` generates a short greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `short?` using the `.Nullable()` combinator.  
- `short` is automatically detected and generated for object properties.  
- `short?` is automatically detected and generated for object properties.  
#### Strings
Use `Fuzzr.String()`.  
- The Default generator generates a string of length greater than or equal to 1 and less than or equal to 10.  
- The overload `Fuzzr.String(int min, int max)` generates a string of length greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The overload `Fuzzr.String(int length)` generates a string of exactly `length` ... erm ... length.  
- Throws an `ArgumentOutOfRangeException` when `length` < 0.  
- The default generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.  
- A version exists for all methods mentioned above that takes a `FuzzrOf<char>` as parameter and then this one will be used to build up the resulting string.  
- Can be made to return `string?` using the `.NullableRef()` combinator.  
- `string` is automatically detected and generated for object properties.  
- `string?` is automatically detected and generated for object properties.  
#### TimeOnlys
Use `Fuzzr.TimeOnly()`.  
- The overload `Fuzzr.TimeOnly(TimeOnly min, TimeOnly max)` generates a TimeOnly greater than or equal to `min` and less than `max`.  
- The default generator is (min = 00:00:00, max = 23:59:59.9999999).  
- Can be made to return `TimeOnly?` using the `.Nullable()` combinator.  
- `TimeOnly` is automatically detected and generated for object properties.  
- `TimeOnly?` is automatically detected and generated for object properties.  
#### TimeSpans
Use `Fuzzr.TimeSpan()`.  
- The overload `Fuzzr.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.  
- The default generator is (max = 1000).  
- Can be made to return `TimeSpan?` using the `.Nullable()` combinator.  
- `TimeSpan` is automatically detected and generated for object properties.  
- `TimeSpan?` is automatically detected and generated for object properties.  
#### UInts
Use `Fuzzr.UInt()`.  
- The overload `Fuzzr.UInt(uint min, uint max)` generates an uint greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `uint?` using the `.Nullable()` combinator.  
- `uint` is automatically detected and generated for object properties.  
- `uint?` is automatically detected and generated for object properties.  
#### ULongs
Use `Fuzzr.ULong()`.  
- The overload `Fuzzr.ULong(ulong min, ulong max)` generates a ulong greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `ulong?` using the `.Nullable()` combinator.  
- `ulong` is automatically detected and generated for object properties.  
- `ulong?` is automatically detected and generated for object properties.  
#### UShorts
Use `Fuzzr.UShort()`.  
- The overload `Fuzzr.UShort(ushort min, ushort max)` generates a ushort greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `ushort?` using the `.Nullable()` combinator.  
- `ushort` is automatically detected and generated for object properties.  
- `ushort?` is automatically detected and generated for object properties.  
### Fuzzing
#### Fuzzr.Constant&lt;T&gt;(T value)
This generator wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined generators.  
#### Fuzzr.Counter(object key)
This generator returns an `int` starting at 1, and incrementing by 1 on each subsequent call.  
#### Fuzzr One
#### Fuzzr One Of
### Fuzzr Extension Methods
#### Ext Fuzzr Apply
#### Ext Fuzzr As Object
#### Ext Fuzzr Many
#### Ext Fuzzr Never Return Null
#### Ext Fuzzr Nullable
#### Ext Fuzzr Nullable Ref
#### Ext Fuzzr Unique
#### Ext Fuzzr Where
### Configuring
#### Configr.Ignore(...)
**Usage:**  
```csharp
 Configr.Ignore(a => a.Name == "Id");
```
Any property matching the predicate will be ignored during generation.  
#### Configr.IgnoreAll()
**Usage:**  
```csharp
 Configr<Thing>.IgnoreAll();
```
Ignore all properties while generating anything.  
#### Configr.Property(...)
**Usage:**  
```csharp
 Configr.Property(a => a.Name == "Id", Fuzzr.Constant(42));
```
Any property matching the predicate will use the specified Fuzzr during generation.  
A utility overload exists that allows one to pass in a value instead of a fuzzr.  
```csharp
 Configr.Property(a => a.Name == "Id", 42);
```
Another overload allows you to create a fuzzr dynamically using a `Func<PropertyInfo, FuzzrOf<T>>` factory method.  
```csharp
 Configr.Property(a => a.Name == "Id", a => Fuzzr.Constant(42));
```
With the same *pass in a value* conveniance helper.  
```csharp
 Configr.Property(a => a.Name == "Id", a => 42);
```
#### Configr&lt;T&gt;.Construct(...)
**Usage:**  
```csharp
 Configr<SomeThing>.Construct(Fuzzr.Constant(42));
```
Subsequent calls to `Fuzzr.One<T>()` will then use the registered constructor.  
Various overloads exist that allow for up to five constructor arguments.  

After that, ... you're on your own.  
#### Configr&lt;T&gt;.Ignore(...)
**Usage:**  
```csharp
 Configr<Thing>.Ignore(s => s.Id);
```
The property specified will be ignored during generation.  
Derived classes generated also ignore the base property.  
#### Configr&lt;T&gt;.IgnoreAll()
**Usage:**  
```csharp
 Configr<Thing>.IgnoreAll();
```
Ignore all properties while generating an object.  
`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.  
#### Configr&lt;T&gt;.Property(...)
**Usage:**  
```csharp
 Configr<Thing>.Property(s => s.Id, Fuzzr.Constant(42));
```
The property specified will be generated using the passed in generator.  
An overload exists which allows for passing a value instead of a generator.  
```csharp
 Configr<Thing>.Property(s => s.Id, 666);
```
Derived classes generated also use the custom property.  
