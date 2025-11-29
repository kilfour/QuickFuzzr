# One
Creates a Fuzzr that produces complete instances of type `T` using QuickFuzzr's automatic construction rules.  

**Signature:**  
```csharp
Fuzzr.One<T>()
```
  

**Usage:**  
```csharp
Fuzzr.One<Person>();
// Results in => { Name: "ddnegsn", Age: 18 }
```
 - Uses `T`'s public parameterless constructor. Parameterized ctors aren't auto-filled unless configured.  
- Primitive properties are generated using their default `Fuzzr` equivalents.  
- Enumerations are filled using `Fuzzr.Enum<T>()`.  
- Object properties are generated where possible.  
- By default, only properties with public setters are auto-generated.  
- Collections remain empty. Lists, arrays, dictionaries, etc. aren't auto-populated.  
- Recursive object creation is off by default.  
 - QuickFuzzr does not automatically detect whether a reference-type property was declared nullable.
  Properties declared like so `public Person? Person { get; set; }` will never have null values, unless configured explicitly.  
- Field generation is not supported.  

**Overloads:**  
- `Fuzzr.One<T>(Func<T> constructor)`:  
  Creates a Fuzzr that produces instances of T by invoking the supplied factory on each generation.  

**Exceptions:**  
- `ConstructionException`: When type T cannot be constructed due to missing default constructor.  
- `InstantiationException`: When type T is an interface and cannot be instantiated.  
- `FactoryConstructionException`: When the factory method returns `null`.  
- `ArgumentNullException`: When the factory method is `null`.  
