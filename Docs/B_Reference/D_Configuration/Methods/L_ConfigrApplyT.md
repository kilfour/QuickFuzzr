# Configr&lt;T&gt;.Apply
Registers an action executed for each generated value of type `T` without modifying the value itself. Use for performing operations like logging, adding to collections, or calling methods that have side effects but don't transform the data.  

**Signature:**  
```csharp
Configr<T>.Apply(Action<T> action)
```
  

**Usage:**  
```csharp
var seen = new List<Person>();
var fuzzr =
    from look in Configr<Person>.Apply(seen.Add)
    from person in Fuzzr.One<Person>()
    from employee in Fuzzr.One<Employee>()
    select (person, employee);
var value = fuzzr.Generate();
// seen now equals 
// [ ( 
//     Person { Name: "ddnegsn", Age: 18 },
//     Employee { Email: "ggnijgna", SocialSecurityNumber: "pkdcsvobs", Name: "xs", Age: 52 }
//) ]
```

**Exceptions:**  
- `ArgumentNullException`: When the provided Action is `null`.  
