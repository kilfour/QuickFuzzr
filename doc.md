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
Person { Name = "ddnegsn", Age = 18 }
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
            Email = $"{firstname}.{lastname}@{email_provider}".ToLower(),
            SocialSecurityNumber = ssn
        };
```
**Output:**  
```
{
    Email: "Jjohn.mccartney@company.com",
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
        Email: "george.harrison@mailings.org",
        SocialSecurityNumber: "953-16-1093",
        Name: "George Harrison",
        Age: 50
    },
    {
        Email: "george.mccartney@mailings.org",
        SocialSecurityNumber: "736-82-8923",
        Name: "George McCartney",
        Age: 48
    },
    {
        Email: "ringo.harrison@mailings.org",
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
**Person/Employee**  
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
        City: "ykgx"
    },
    Email: "dratnq",
    SocialSecurityNumber: "ggygun",
    Name: "v",
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
// Results in => "115-27-1722"
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
// Results in => { Name: "xtvtbadfqx", Age: 720 }
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
// Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
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
{  Name: "ljduv", SubFolder: null }
```
You can however influence how deep the rabbit hole goes,
by adding call to `Configr<Folder>.Depth(min, max)`:  
```csharp
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
// look for a coaches that can be assigned to the course
//   - WithDefault results in <null> if the collection is empty
let elligibleCoaches = coaches.Where(a => a.IsSuitableFor(course) && a.IsAvailableFor(course))
from coachToAssign in Fuzzr.OneOf(elligibleCoaches).WithDefault()
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
