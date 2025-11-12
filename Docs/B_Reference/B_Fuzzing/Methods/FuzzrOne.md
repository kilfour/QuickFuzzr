# Fuzzr.One&lt;T&gt;()
Creates a fuzzr that produces complete instances of type `T` using QuickFuzzr's automatic construction rules:   
**Usage:**  
```csharp
Fuzzr.One<Person>();
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
