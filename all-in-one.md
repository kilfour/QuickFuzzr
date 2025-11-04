# QuickFuzzr
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
Output:  
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
Calling `.Generate()` outputs:  
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
Output:     
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
var personConfigr =
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
    from employeeCfg in personConfigr
    from housedEmployee in Fuzzr.One<HousedEmployee>()
    select housedEmployee;
```
The Output:  
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
Output:      
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
  
**Configr.Construct:** Best for reusable configurations;  
```csharp
var vowel = Fuzzr.OneOf('a', 'e', 'o', 'u', 'i');
from cfg in Configr<PersonRecord>.Construct(Fuzzr.String(vowel, 2, 10), Fuzzr.Int())
from person in Fuzzr.One<PersonRecord>() // <= 'One' now works, thanks to Configr
select person;
// Results in => { Name = "aaoaeuoa", Age = 76 }
```
**Explicit generation**: Most straightforward for one-off cases.  
```csharp
from name in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
from age in Fuzzr.Int(-100, 0)
select new PersonRecord(name, age);
// Results in => { Name = "George", Age = -86 }
```
**Factory method:** Useful when you need the object wrapped in `FuzzrOf<T>`.  
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
