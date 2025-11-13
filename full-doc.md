## Guide
### Contents

- [Your First Fuzzr][YourFirstFuzzr]
- [On Composition][OnComposition]
- [Beautifully Carved Objects][BeautifullyCarvedObjects]
- [Through the Looking Glass][ThroughTheLookingGlass]
- [TheFinalShowcase][TheFinalShowcase]
  

[YourFirstFuzzr]: #your-first-fuzzr

[OnComposition]: #on-composition

[BeautifullyCarvedObjects]: #beautifully-carved-objects

[ThroughTheLookingGlass]: #through-the-looking-glass

[TheFinalShowcase]: #the-final-showcase
### Your First Fuzzr
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
and fills them in using the default fuzzrs.  
#### Your First Customization
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
#### Composable Fuzzrs
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
    Email: "john.mccartney@company.com",
    SocialSecurityNumber: "761-65-2228",
    Name: "John McCartney",
    Age: 69
}
```
In this example the lists used by `OneOf` are declared outside of the fuzzr.
I just used `string[]`'s but the data could easily be loaded from a file for instance.  
#### Please, Sir, I Want Some More.
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
#### The Strand Union Workhouse
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
        $"{firstname}.{lastname}@{email_provider}".ToLower());
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
    Email: "george.lennon@company.com",
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

Generate just addresses (or `Person`, `Employee`, etc.):
  
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
        Email: "paul.mccartney@mailings.org",
        SocialSecurityNumber: "621-54-5020",
        Name: "Paul McCartney",
        Age: 27
    },
    {
        Address: {
            Street: "Victoria Street",
            City: "London"
        },
        Email: "george.harrison@freemail.net",
        SocialSecurityNumber: "535-80-4278",
        Name: "George Harrison",
        Age: 34
    },
    {
        Address: {
            Street: "Kings Road",
            City: "Manchester"
        },
        Email: "paul.star@company.com",
        SocialSecurityNumber: "428-67-7239",
        Name: "Paul Star",
        Age: 11
    }
)
```
As you can see this reuses the previous Fuzzr, generates a *normal* `HousedEmployee`,
another one that is guaranteed to live in London, and finally an underaged one.  
### On Composition
In the previous chapter, the examples kind of went from zero to sixty in under three, so let's step back a bit and have a look under the hood.  
#### Fuzzr
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
As an example of that, here is another social security fuzzr, this time using `char`'s and `string`'s:  
```csharp
var digit = Fuzzr.Char('0', '9');
var ssnFuzzr =
    from a in Fuzzr.String(digit, 3)
    from b in Fuzzr.String(digit, 2)
    from c in Fuzzr.String(digit, 4)
    select $"{a}-{b}-{c}";
// Results in => "115-27-1722"
```
#### Configr
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
#### Extension Methods
Lastly, there are some extension methods defined for `FuzzrOf<T>`. I think the most obvious one is `.Many(...)`.  
These extension methods wrap the `FuzzrOf<T>` they are attached to and again, influence generation.  
In the case of `.Many(...)`, it runs the original `Fuzzr` as many times as specified in the arguments and returns the resulting values as an `IEnumerable<T>`.  

Example:  
```csharp
Fuzzr.Int().Many(3);
// Results in => [ 67, 14, 13 ]
```
##### A Word of Caution
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
### Beautifully Carved Objects
Now that we understand how composition works in QuickFuzzr, let's examine how this concept is applied to object generation.  
#### From Fragments to Forms
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
#### Where QuickFuzzr Draws the Line
- By default, only properties with public setters are auto-generated.  
- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.  
- Recursive object creation is off by default.  

**Note:** All of QuickFuzzrs defaults can be overridden using `Configr`.  
#### Where `One` Is Not Enough
##### Construction
Consider:  
```csharp
public record PersonRecord(string Name, int Age);
```
This record does not have a default constructor, so `Fuzzr.One<PersonRecord>()`
will throw an exception with the following message if used as is:  
```text
Cannot generate instance of PersonRecord.
Possible solutions:
- Add a parameterless constructor
- Register a custom constructor: Configr<PersonRecord>.Construct(...)
- Use explicit generation: from x in Fuzzr.Int() ... select new PersonRecord(x)
- Use the factory method overload: Fuzzr.One<T>(Func<T> constructor)
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
##### Property Access
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
##### Filling Collections
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
### Through the Looking Glass
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
    public string Name { get; set; } = string.Empty;
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
        from fs in Fuzzr.One<FileEntry>().Many(1, 2) select fs.ToList())
    from folders in Configr<FolderEntry>.Property(a => a.Folders,
        from fs in Fuzzr.One<FolderEntry>().Many(1, 2) select fs.ToList())
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
When the counter hits zero, the fuzzr yields null (or an empty list), and the story ends right there.

It also means you can mix these with inheritance and collection combinators.  
And depth is local, not global: one deep branch does not force all others to go equally deep.  
### The Final Showcase
So, ..., if we have a domain that contains GenericIdentities, ValueObjects, Aggregates, etc.,
how *exactly* do we handle that in practice?

This example uses the [HorsesForCourses](https://github.com/kilfour/HorsesForCourses) example domain.  
*Note:* `Admin` is a system user authorized for mutations, defined elsewhere in the domain model.  
##### Building Blocks
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
##### Looking Back
This showcase demonstrates how QuickFuzzr can:
- Generate fully interconnected domain aggregates with bidirectional relationships
- Respect domain rules and business logic
- Handle identity generation and uniqueness constraints  
- Compose complex objects from simple building blocks
- Work with real-world domain patterns (entities, value objects, aggregates)  
## Reference
This reference provides a **complete, factual overview** of QuickFuzzr's public API.
It lists all available fuzzrs, configuration points, and extension methods, organized by category.  
Each entry includes a concise description of its purpose and behavior,
serving as a quick lookup for day-to-day use or library integration.

All examples and summaries are real, verified through executable tests, ensuring what you see here is exactly what QuickFuzzr does.

QuickFuzzr exposes three kinds of building blocks: `FuzzrOf<T>` for value production, `Configr` for generation behavior, and extension methods for modifying fuzzrs.  
Everything in this reference fits into one of these three roles.

If you're looking for examples or background explanations, see the guide or cookbook.
  
### Contents

- [Fuzzing][Fuzzing]
- [Configuration][Configuring]
- [Fuzzr Extension Methods][FuzzrExtensionMethods]
- [Primitive Fuzzrs][PrimitiveFuzzrs]
  

[PrimitiveFuzzrs]: #primitive-fuzzrs

[Fuzzing]: #fuzzing

[FuzzrExtensionMethods]: #fuzzr-extension-methods

[Configuring]: #configuring
### Fuzzing
This section lists the core fuzzrs responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a `FuzzrOf<T>` and can be composed using standard LINQ syntax.  
#### Contents
| Fuzzr| Description |
| -| - |
| [One](#one)| Creates a fuzzr that produces an instances of type `T`. |
| [OneOf](#oneof)| Randomly selects one of the provided values. |
| [Shuffle](#shuffle)| Creates a fuzzr that randomly shuffles an input sequence. |
| [Counter](#counter)| Generates a sequential integer per key, starting at 1. |
| [Constant](#constant)| Wraps a fixed value in a fuzzr, producing the same result every time. |
#### One
Creates a fuzzr that produces complete instances of type `T` using QuickFuzzr's automatic construction rules.  

**Signature:**  
```csharp
Fuzzr.One<T>()
```
  

**Usage:**  
```csharp
Fuzzr.One<Person>();
// Results in => { Name: "ddnegsn", Age: 18 }
```
 - Uses `T`'s public parameterless constructor. Parameterized ctors aren't auto-filled.  
- Primitive properties are generated using their default `Fuzzr` equivalents.  
- Enumerations are filled using `Fuzzr.Enum<T>()`.  
- Object properties are generated where possible.  
- By default, only properties with public setters are auto-generated.  
- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.  
- Recursive object creation is off by default.  
- Field generation is not supported.  

**Overloads:**  
- `Fuzzr.One<T>(Func<T> constructor)`:  
  Creates a fuzzr that produces instances of T by invoking the supplied factory on each generation.  

**Exceptions:**  
- `ConstructionException`: When type T cannot be constructed due to missing default constructor.  
- `InstantiationException`: When type T is an interface and cannot be instantiated.  
- `NullReferenceException`:  
  - When the factory method returns null.  
  - When the factory method is null.  
#### OneOf
Creates a fuzzr that randomly selects one value or fuzzr from the provided options.  

**Signature:**  
```csharp
Fuzzr.OneOf(params T[] values)
```
  

**Usage:**  
```csharp
 Fuzzr.OneOf("a", "b", "c");
```
- Selection is uniform unless weights are specified (see below).  

**Overloads:**  
- `Fuzzr.OneOf(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  
- `Fuzzr.OneOf(params FuzzrOf<T>[] fuzzrs)`:  
  Randomly selects and executes one of the provided fuzzrs.  
- `Fuzzr.OneOf(params (int Weight, T Value)[] weightedValues)`:  
  Selects a value using weighted probability. The higher the weight, the more likely the value is to be chosen.  
```csharp
 Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c"));
```
- `Fuzzr.OneOf(params (int Weight, FuzzrOf<T> fuzzr)[] weightedFuzzrs)`:  
  Like the weighted values overload, but applies weights to fuzzrs.  

**Exceptions:**  
  - `OneOfEmptyOptionsException`: When trying to choose from an empty collection.  
  - `NegativeWeightException`: When one or more weights are negative.  
  - `ZeroTotalWeightException`: When the total of all weights is zero or negative.  
  - `ArgumentNullException`: When the provided sequence is null.  
#### Shuffle
Creates a fuzzr that produces a random permutation of the provided sequence.  
Use for randomized ordering, unbiased sampling without replacement.
  

**Signature:**  
```csharp
Fuzzr.Shuffle(params T[] values)
```
  

**Usage:**  
```csharp
Fuzzr.Shuffle("John", "Paul", "George", "Ringo");
// Results in => ["Paul", "Ringo", "John", "George"]
```

**Overloads:**  
- `Shuffle<T>(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  

**Exceptions:**  
  - `ArgumentNullException`: When the input collection is `null`.  
#### Counter
This fuzzr returns an `int` starting at 1, and incrementing by 1 on each call.  
Useful for generating unique sequential IDs or counters.  
  

**Signature:**  
```csharp
Fuzzr.Counter(object key)
```
  

**Usage:**  
```csharp
Fuzzr.Counter("the-key").Many(5).Generate();
// Returns => [1, 2, 3, 4, 5]
```
- Each `key` maintains its own independent counter sequence.  
- Counter state resets between separate `Generate()` calls.  
- Works seamlessly in LINQ chains and with .Apply(...) to offset or transform the sequence.  

**Exceptions:**  
- `ArgumentNullException`: When the provided key is null.  
#### Constant
This fuzzr wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined fuzzrs.  

**Signature:**  
```csharp
Fuzzr.Constant(T value)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(41);
// Results in => 42
```
### Configuring
`Configr` provides a fluent API to influence how QuickFuzzr builds objects.
Use it to set global defaults, customize properties, control recursion depth,
select derived types, or wire dynamic behaviors that apply when calling `Fuzzr.One<T>()`.
  
#### Contents
| Configr| Description |
| -| - |
| [Configr&lt;T&gt;.Ignore(Expression&lt;Func&lt;T, TProperty&gt;&gt; expr)](#configrtignoreexpressionfunct-tproperty-expr)|   |
| [Configr.Ignore(Func&lt;PropertyInfo, bool&gt; predicate)](#configrignorefuncpropertyinfo-bool-predicate)|   |
| [Configr&lt;T&gt;.IgnoreAll()](#configrtignoreall)|   |
| [Configr.IgnoreAll()](#configrignoreall)|   |
| [Configr&lt;T&gt;.Property(...)](#configrtproperty)|   |
| [Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)](#configrpropertytpropertyfuncpropertyinfo-bool-predicate-fuzzroftproperty-fuzzr)|   |
| [Configr&lt;T&gt;.Construct(FuzzrOf&lt;T1&gt; arg1)](#configrtconstructfuzzroft1-arg1)|   |
| [Configr&lt;T&gt;AsOneOf(params Type[] types)](#configrtasoneofparams-type-types)|   |
| [Configr&lt;T&gt;.EndOn&lt;TEnd&gt;()](#configrtendontend)|   |
| [Configr&lt;T&gt;.Depth(int min, int max)](#configrtdepthint-min-int-max)|   |
| [Configr.RetryLimit(int limit)](#configrretrylimitint-limit)|   |
| [Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)](#configrtwithtvaluefuzzroftvalue-fuzzr-functvalue-fuzzrofintent-configrfactory)|   |
| [Configr.Primitive&lt;T&gt;(this FuzzrOf&lt;T&gt; fuzzr)](#configrprimitivetthis-fuzzroft-fuzzr)|   |
| [Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) / Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)](#configrenablepropertyaccessforpropertyaccess-propertyaccess-/-configrdisablepropertyaccessforpropertyaccess-propertyaccess)|   |
#### Configr&lt;T&gt;.Ignore(Expression&lt;Func&lt;T, TProperty&gt;&gt; expr)

**Usage:**  
```csharp
from ignore in Configr<Person>.Ignore(a => a.Name)
from person in Fuzzr.One<Person>()
select person;
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
The property specified will be ignored during generation.  
Derived classes generated also ignore the base property.  
#### Configr.Ignore(Func&lt;PropertyInfo, bool&gt; predicate)
Skips all properties matching the predicate across all types during generation.  
Use to exclude recurring patterns like identifiers, foreign keys, or audit fields.  

**Usage:**  
```csharp
from ignore in Configr.Ignore(a => a.Name == "Name")
from person in Fuzzr.One<Person>()
from fileEntry in Fuzzr.One<FileEntry>()
select (person, fileEntry);
// Results in => 
// ( Person { Name: "", Age: 67 }, FileEntry { Name: "" } )
```
#### Configr&lt;T&gt;.IgnoreAll()

**Usage:**  
```csharp
from ignore in Configr<Person>.IgnoreAll()
from person in Fuzzr.One<Person>()
from address in Fuzzr.One<Address>()
select (person, address);
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
Ignore all properties while generating an object.  
`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.  
#### Configr.IgnoreAll()
Ignore all properties while generating anything.  

**Usage:**  
```csharp
from ignore in Configr.IgnoreAll()
from person in Fuzzr.One<Person>()
from address in Fuzzr.One<Address>()
select (person, address);
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
#### Configr&lt;T&gt;.Property(...)

**Usage:**  
```csharp
 Configr<Thing>.Property(s => s.Id, Fuzzr.Constant(42));
```
- The property specified will be generated using the passed in fuzzr.  
- An overload exists which allows for passing a value instead of a fuzzr.  
```csharp
 Configr<Thing>.Property(s => s.Id, 666);
```
- Derived classes generated also use the custom property.  
- Trying to configure a field throws an exception with the following message:  
```text
Cannot configure expression 'a => a.Name'.
It does not refer to a property.
Fields and methods are not supported by default.
Possible solutions:
- Use a property selector (e.g. a => a.PropertyName).
- Then pass it to Configr<PersonOutInTheFields>.Property(...) to configure generation.
```
#### Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)

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
#### Configr&lt;T&gt;.Construct(FuzzrOf&lt;T1&gt; arg1)
Configures a custom constructor for type T, used when Fuzzr.One<T>() is called.
Useful for records or classes without parameterless constructors or when `T` has multiple constructors
and you want to control which one is used during fuzzing.  
  

**Usage:**  
```csharp
Configr<SomeThing>.Construct(Fuzzr.Constant(42));
```

**Overloads:**  
```csharp
Construct<T1,T2>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2)
```
```csharp
Construct<T1,T2,T3>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3)
```
```csharp
Construct<T1,T2,T3,T4>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4)
```
```csharp
Construct<T1,T2,T3,T4,T5>(FuzzrOf<T1> arg1, FuzzrOf<T2> arg2, FuzzrOf<T3> arg3, FuzzrOf<T4> arg4, FuzzrOf<T5> arg5)
```

**Exceptions:**  
- `ArgumentNullException`: If one of the `TArg` parameters is null.  
- `InvalidOperationException`: If no matching constructor is found on type T.  
#### Configr&lt;T&gt;AsOneOf(params Type[] types)
Configures inheritance resolution for BaseType, 
allowing QuickFuzzr to randomly select one of the specified derived types when generating instances.  

Useful when generating domain hierarchies where multiple concrete subtypes exist.  
  

**Usage:**  
```csharp
var personFuzzr =
    from asOneOf in Configr<Person>.AsOneOf(typeof(Person), typeof(Employee))
    from item in Fuzzr.One<Person>()
    select item;
personFuzzr.Many(2).Generate();
// Results in =>
// [
//     Employee {
//         Email: "dn",
//         SocialSecurityNumber: "gs",
//         Name: "etggni",
//         Age: 38
//     },
//     Person { Name: "avpkdc", Age: 70 }
// ]
```

**Exceptions:**  
- `EmptyDerivedTypesException`: When no types are provided.  
- `DuplicateDerivedTypesException`: When the list of derived types contains duplicates.  
  - `DuplicateDerivedTypesException`: When the list of derived types contains duplicates.  
- `DerivedTypeNotAssignableException`: If any listed type is not a valid subclass of `BaseType`.  
- `DerivedTypeIsNullException`: If any listed type is `null`.  
#### Configr&lt;T&gt;.EndOn&lt;TEnd&gt;()
  
Configures a recursion stop condition for type `T`, instructing QuickFuzzr to generate `TEnd` instances instead of continuing deeper.
Useful for defining explicit *end* types in recursive object graphs, preventing infinite nesting and keeping structure depth under control.
  

**Usage:**  
```csharp
from ending in Configr<Turtle>.EndOn<MoreTurtles>()
from turtle in Fuzzr.One<Turtle>()
select turtle;
// Results in => 
// MoreTurtles { Down: null }
```
With depth constraints, QuickFuzzr respects the specified min/max depth when applying the `EndOn<TEnd>()` rule.  
```csharp
from depth in Configr<Turtle>.Depth(1, 3)
from ending in Configr<Turtle>.EndOn<MoreTurtles>()
from turtle in Fuzzr.One<Turtle>()
select turtle;
// Results in => 
// Turtle { Down: Turtle { Down: MoreTurtles { Down: null } } }
```

**Exceptions:**  
- `DerivedTypeNotAssignableException`: When `TEnd` is not assignable to `T`.  
#### Configr&lt;T&gt;.Depth(int min, int max)
Configures depth constraints for type `T` to control recursive object graph generation. 
  

**Usage:**  
```csharp
Configr<Turtle>.Depth(2, 5);
// Results in =>
// Turtle { Down: { Down: { Down: { Down: null } } } }
```
Subsequent calls to `Fuzzr.One<T>()` will generate between 2 and 5 nested levels of `Turtle` instances,
depending on the random draw and available recursion budget.  
Depth is per type, not global. Each recursive type manages its own budget.
  

**Exceptions:**  
- `ArgumentOutOfRangeException`: When min is negative.  
- `ArgumentOutOfRangeException`: When max is lesser than min  
#### Configr.RetryLimit(int limit)

**Usage:**  
```csharp
 Configr.RetryLimit(256);
```
- Sets the global retry limit used by fuzzrs.  
- Throws when trying to set limit to a value lesser than 1.  
- Throws when trying to set limit to a value greater than 1024.  
```text
Invalid retry limit value: 1025
Allowed range: 1-1024
Possible solutions:
- Use a value within the allowed range
- Check for unintended configuration overrides
- If you need more, consider revising your fuzzr logic instead of increasing the limit
```
#### Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)
Applies configuration for type `T` based on a value generated from another fuzzr.  
#### Configr.Primitive&lt;T&gt;(this FuzzrOf&lt;T&gt; fuzzr)
Registers a global default fuzzr for primitive types.
Use this to override how QuickFuzzr generates built-in types across all automatically created objects.
  

**Usage:**  
```csharp
from cfgInt in Configr.Primitive(Fuzzr.Constant(42))
from person in Fuzzr.One<Person>()
from timeslot in Fuzzr.One<TimeSlot>()
select (person, timeslot);
// Results in => 
// ( Person { Name: "ddnegsn", Age: 42 }, TimeSlot { Day: Monday, Time: 42 } )
```
Replacing a primitive fuzzr automatically impacts its nullable counterpart.  

**Overloads:**  
- `Primitive<T>(this FuzzrOf<T?> fuzzr)`:  
  Registers a global default fuzzr for nullable primitives `T?`, overriding all nullable values produced across generated objects.  
```csharp
from cfgString in Configr.Primitive(Fuzzr.Constant<int?>(42))
from person in Fuzzr.One<Person>()
from nullablePerson in Fuzzr.One<NullablePerson>()
select (person, nullablePerson);
// Results in => 
// ( Person { Name: "cmu", Age: 66 }, NullablePerson { Name: "ycqa", Age: 42 } )
```
  Replacing a nullable primitive fuzzr does not impacts it's non-nullable counterpart.  
- `Fuzzr.Primitive(this FuzzrOf<string> fuzzr)`:  
  Registers a global default fuzzr for strings, overriding all string values produced across generated objects.  
```csharp
from cfgString in Configr.Primitive(Fuzzr.Constant("FIXED"))
from person in Fuzzr.One<Person>()
from address in Fuzzr.One<Address>()
select (person, address);
// Results in => 
// ( Person { Name: "FIXED", Age: 67 }, Address { Street: "FIXED", City: "FIXED" } )
```
#### Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) / Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)

**Usage:**  
```csharp
from enable in Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)
from person1 in Fuzzr.One<PrivatePerson>()
from disable in Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)
from person2 in Fuzzr.One<PrivatePerson>()
select (person1, person2);
// Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
```
Enables and then disables generation for init-only properties.  
Updates state flags using bitwise enable/disable semantics.  
### Fuzzr Extension Methods
QuickFuzzr provides a collection of extension methods that enhance the expressiveness and composability of `FuzzrOf<T>`.
These methods act as modifiers, they wrap existing fuzzrs to alter behavior, add constraints,
or chain side-effects without changing the underlying LINQ-based model.
  
#### Contents
| Method| Description |
| -| - |
| [Apply(this FuzzrOf&lt;T&gt; fuzzr, Action&lt;T&gt; action)](#applythis-fuzzroft-fuzzr-actiont-action)| Executes a side-effect for, or transform each value generated by the wrapped fuzzr. |
| [AsObject(this FuzzrOf&lt;T&gt; fuzzr)](#asobjectthis-fuzzroft-fuzzr)| Boxes generated values as `object` without modifying them. |
| [Many(this FuzzrOf&lt;T&gt; fuzzr, int number)](#manythis-fuzzroft-fuzzr-int-number)| Produces a number of values from the wrapped fuzzr. |
| [NeverReturnNull(this FuzzrOf&lt;T?&gt; fuzzr)](#neverreturnnullthis-fuzzroft?-fuzzr)| Filters out null values, retrying until a non-null value is produced or the retry limit is exceeded. |
| [Nullable(this FuzzrOf&lt;T&gt; fuzzr)](#nullablethis-fuzzroft-fuzzr)| Converts a non-nullable value-type fuzzr into a nullable one with a default 20% null probability. |
| [NullableRef(this FuzzrOf<T> fuzzr)](#nullablerefthis-fuzzroft-fuzzr)| Wraps a reference-type fuzzr to sometimes return null (default 20% chance). |
| [Shuffle&lt;T&gt;(this FuzzrOf&lt;IEnumerable&lt;T&gt;&gt; source)](#shuffletthis-fuzzrofienumerablet-source)| Randomly shuffles the sequence produced by the source fuzzr. |
| [Unique&lt;T&gt;(this FuzzrOf&lt;T&gt; fuzzr, object key)](#uniquetthis-fuzzroft-fuzzr-object-key)| Ensures all generated values are unique within the given key scope. |
| [Where(this FuzzrOf&lt;T&gt; fuzzr, Func&lt;T,bool&gt; predicate)](#wherethis-fuzzroft-fuzzr-functbool-predicate)| Filters generated values so only values satisfying the predicate are returned. |
| [ExtFuzzr.WithDefault(this FuzzrOf&lt;T&gt; fuzzr, T def = default)](#extfuzzrwithdefaultthis-fuzzroft-fuzzr-t-def-=-default)| Returns a default value when the underlying fuzzr fails due to empty choices. |
#### Apply(this FuzzrOf&lt;T&gt; fuzzr, Action&lt;T&gt; action)
Executes a side-effect per generated value without altering it.  

**Usage:**  
```csharp
var seen = new List<int>();
var fuzzr = Fuzzr.Int().Apply(seen.Add);
var value = fuzzr.Generate();
// seen now equals [ 67 ]
```

**Overloads:**  
- `Apply(this FuzzrOf<T> fuzzr, Func<T,T> func)`  
  Transforms generated values while preserving generation context.  
```csharp
Fuzzr.Constant(41).Apply(x => x + 1);
// Results in => 42
```

**Exceptions:**  
- `NullReferenceException`: When the provided `Action` or `Func` is null.  
#### AsObject(this FuzzrOf&lt;T&gt; fuzzr)
Boxes generated values as object without altering them.  

**Usage:**  
```csharp
Fuzzr.Constant(42).AsObject();
// Results in => 42
```
#### Many(this FuzzrOf&lt;T&gt; fuzzr, int number)
Produces a fixed number of values from a fuzzr.  

**Usage:**  
```csharp
Fuzzr.Constant(6).Many(3);
// Results in => [6, 6, 6]
```

**Overloads:**  
- `Many(this FuzzrOf<T> fuzzr, int min, int max)`  
  Produces a variable number of values within bounds.  
#### NeverReturnNull(this FuzzrOf&lt;T?&gt; fuzzr)
Filters out nulls from a nullable fuzzr, retrying up to the retry limit.  

**Usage:**  

**Exceptions:**  
- `NonNullValueExhaustedException`: When all attempts result in null.  
#### Nullable(this FuzzrOf&lt;T&gt; fuzzr)
Wraps a value type fuzzr to sometimes yield null values.  

**Usage:**  
#### NullableRef(this FuzzrOf<T> fuzzr)
Wraps a reference type fuzzr to sometimes yield null values.  
#### Shuffle&lt;T&gt;(this FuzzrOf&lt;IEnumerable&lt;T&gt;&gt; source)
#### Unique&lt;T&gt;(this FuzzrOf&lt;T&gt; fuzzr, object key)
Using the `.Unique(object key)` extension method.  
- Makes sure that every generated value is unique.  
- When asking for more unique values than the fuzzr can supply, an exception is thrown.  
- Multiple unique fuzzrs can be defined in one 'composed' fuzzr, without interfering with eachother by using a different key.  
- When using the same key for multiple unique fuzzrs all values across these fuzzrs are unique.  
- An overload exist taking a function as an argument allowing for a dynamic key.  
#### Where(this FuzzrOf&lt;T&gt; fuzzr, Func&lt;T,bool&gt; predicate)
Filters generated values to those satisfying the predicate.  

**Exceptions:**  
- `PredicateUnsatisfiedException`: When no value satisfies the predicate within the retry limit.  
#### ExtFuzzr.WithDefault(this FuzzrOf&lt;T&gt; fuzzr, T def = default)
Returns the (optionally) provided default value instead of throwing when the underlying fuzzr fails due to empty choices.  
### Primitive Fuzzrs
QuickFuzzr includes built-in fuzzrs for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex fuzzrs are composed, and are used automatically when generating object properties.

> *All primitive fuzzrs automatically drive object property generation.
> Nullable and non-nullable variants are both supported.
> Each fuzzr also supports `.Nullable(...)` / `.NullableRef(...)` as appropriate.*
  
| Fuzzr| Description |
| -| - |
| [Booleans](#booleans)| Generates random `true` or `false` values.  |
| [Bytes](#bytes)| Produces random bytes in the range 0-255 or within a custom range. |
| [Chars](#chars)| Generates random lowercase letters or characters within a specified range. |
| [DateOnlys](#dateonlys)| Creates `DateOnly` values between 1970-01-01 and 2020-12-31 (by default). |
| [DateTimes](#datetimes)| Produces `DateTime` values between 1970-01-01 and 2020-12-31 (inclusive), snapped to whole seconds. |
| [Decimals](#decimals)| Generates random decimal numbers (default 1-100). |
| [Doubles](#doubles)| Generates random double-precision numbers (default 1-100). |
| [Enums](#enums)| Randomly selects a defined member of an enum type. |
| [Floats](#floats)| Generates random single-precision numbers (default 1-100). |
| [Guids](#guids)| Produces non-empty random `Guid` values. |
| [Halfs](#halfs)| Generates random 16-bit floating-point numbers (default 1-100). |
| [Ints](#ints)| Produces random integers (default 1-100). |
| [Longs](#longs)| Generates random 64-bit integers (default 1-100). |
| [Shorts](#shorts)| Generates random 16-bit integers (default 1-100). |
| [Strings](#strings)| Creates random lowercase strings (default length 1-10). |
| [TimeOnlys](#timeonlys)| Produces random times between midnight and 23:59:59. |
| [TimeSpans](#timespans)| Generates random durations up to 1000 ticks by default. |
| [UInts](#uints)| Produces unsigned integers (default 1-100). |
| [ULongs](#ulongs)| Generates unsigned 64-bit integers (default 1-100). |
| [UShorts](#ushorts)| Produces unsigned 16-bit integers (default 1-100). |
#### Booleans
Use `Fuzzr.Bool()`.  
- Generates True or False.  
#### Bytes
Use `Fuzzr.Byte()`.  
- The default fuzzr produces a `byte` in the full range (`0`-`255`).  
- The overload `Fuzzr.Byte(int min, int max)` generates a value greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- Throws an `ArgumentOutOfRangeException` when `min` < `byte.MinValue` (i.e. `< 0`).  
- Throws an `ArgumentOutOfRangeException` when `max` > `byte.MaxValue` (i.e. `> 255`).  
- When `min == max`, the fuzzr always returns that exact value.  
- Boundary coverage: over time, values at both ends of the interval should appear.  
#### Chars
Use `Fuzzr.Char()`.  
- The overload `Fuzzr.Char(char min, char max)` generates a char greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr always generates a char between lower case 'a' and lower case 'z'.  
#### DateOnlys
Use `Fuzzr.DateOnly()`.  
- The overload `Fuzzr.DateOnly(DateOnly min, DateOnly max)` generates a DateOnly greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = new DateOnly(1970, 1, 1), max = new DateOnly(2020, 12, 31)).  
#### DateTimes
Use `Fuzzr.DateTime()`.  
- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a `DateTime` in the inclusive range [min, max], snapped to whole seconds.  
- Generated values are snapped to whole seconds.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)) inclusive, snapped to whole seconds.  
#### Decimals
Use `Fuzzr.Decimal()`.  

- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal in the range [min, max) (min inclusive, max exclusive).  
- The overload `Decimal(int precision)` generates a decimal with up to `precision` decimal places.  
- The overload `Decimal(decimal min, decimal max, int precision)` generates a decimal in the range [min, max) (min inclusive, max exclusive), with up to `precision` decimal places.  
- When `min == max`, the fuzzr always returns that exact value.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100, precision = 2).  
#### Doubles
Use `Fuzzr.Double()`.  
- The overload `Fuzzr.Double(double min, double max)` generates a double greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
#### Enums
Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.  
> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.  
- The default fuzzr just picks a random value from all enumeration values.  
- Passing in a non Enum type for T throws an ArgumentException.  
#### Floats
Use `Fuzzr.Float()`.  
- The overload `Fuzzr.Float(float min, float max)` generates a float greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
#### Guids
Use `Fuzzr.Guid()`. *There is no overload.*  
- The default fuzzr never generates Guid.Empty.  
- `Fuzzr.Guid()` is deterministic when seeded.  
#### Halfs
Use `Fuzzr.Half()`.  

- The overload Fuzzr.Half(Half min, Half max) generates a half-precision floating-point number greater than or equal to `min` and less than `max`.
  *Note:* Due to floating-point rounding, max may occasionally be produced.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = (Half)1, max = (Half)100).  
#### Ints
Use `Fuzzr.Int()`.  
- The overload `Fuzzr.Int(int min, int max)` generates an int greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
#### Longs
Use `Fuzzr.Long()`.  
- The overload `Fuzzr.Long(long min, long max)` generates a long greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
#### Shorts
Use `Fuzzr.Short()`.  
- The overload `Fuzzr.Short(short min, short max)` generates a short greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
#### Strings
Use `Fuzzr.String()`.  
- The Default fuzzr generates a string of length greater than or equal to 1 and less than or equal to 10.  
- The overload `Fuzzr.String(int min, int max)` generates a string of length greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The overload `Fuzzr.String(int length)` generates a string of exactly `length` ... erm ... length.  
- Throws an `ArgumentOutOfRangeException` when `length` < 0.  
- The default fuzzr always generates every char element of the string to be between lower case 'a' and lower case 'z'.  
- A version exists for all methods mentioned above that takes a `FuzzrOf<char>` as parameter and then this one will be used to build up the resulting string.  
#### TimeOnlys
Use `Fuzzr.TimeOnly()`.  
- The overload `Fuzzr.TimeOnly(TimeOnly min, TimeOnly max)` generates a TimeOnly greater than or equal to `min` and less than `max`.  
- **Default:** min = 00:00:00, max = 23:59:59.9999999).  
#### TimeSpans
Use `Fuzzr.TimeSpan()`.  
- The overload `Fuzzr.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.  
- **Default:** max = 1000).  
#### UInts
Use `Fuzzr.UInt()`.  
- The overload `Fuzzr.UInt(uint min, uint max)` generates an uint greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
#### ULongs
Use `Fuzzr.ULong()`.  
- The overload `Fuzzr.ULong(ulong min, ulong max)` generates a ulong greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
#### UShorts
Use `Fuzzr.UShort()`.  
- The overload `Fuzzr.UShort(ushort min, ushort max)` generates a ushort greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
## Cooking Up a Fuzzr
### Contents

- [A Deep Dark Forest][ADeepDarkForest]
  

[ADeepDarkForest]: #a-deep-dark-forest
### A Deep Dark Forest
Using `Fuzzr<T>.Depth()` together with the `Fuzzr<T>.AsOneOf(...)` combinator and `Fuzzr<T>.EndOn<TEnd>()`
allows you to build tree type hierarchies.  
Given the canonical abstract `Tree`, concrete `Branch` and `Leaf` example model:   
```csharp
public abstract class Tree { }
public class Leaf : Tree { }
public class Branch : Tree
{
    public Tree? Left { get; set; }
    public Tree? Right { get; set; }
}
```
We can generate a forest like so:  
```csharp
    from depth in Configr<Tree>.Depth(2, 5)
    from inheritance in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
    from terminator in Configr<Tree>.EndOn<Leaf>()
    from tree in Fuzzr.One<Tree>()
    select tree;
```
**Output:**  
```text
Branch
  ├── Branch
  │   ├── Leaf
  │   └── Branch
  │       ├── Leaf
  │       └── Leaf
  └── Leaf
```
