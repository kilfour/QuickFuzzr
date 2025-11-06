# On Composition
In the previous chapter, the examples kind of went from zero to sixty in under three, so let's step back a bit and have a look under the hood.  
## Fuzzr
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
## Configr
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
## Extension Methods
Lastly, there are some extension methods defined for `FuzzrOf<T>`. I think the most obvious one is `.Many(...)`.  
These extension methods wrap the `FuzzrOf<T>` they are attached to and again, influence generation.  
In the case of `.Many(...)`, it runs the original `Fuzzr` as many times as specified in the arguments and returns the resulting values as an `IEnumerable<T>`.  

Example:  
```csharp
Fuzzr.Int().Many(3);
// Results in => [ 67, 14, 13 ]
```
### A Word of Caution
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
