# Configr.Combine
Combines multiple configuration operations into a single Configr.  
The operations are applied in argument order.  

**Signature:**  
```csharp
Configr.Combine(params FuzzrOf<Intent>[] configrs)
```
  

**Usage:**  
```csharp
Configr.Combine(
    Configr<Person>.Property(person => person.Name, "Arthur"),
    Configr<Person>.Property(person => person.Age, 42));
```
- Configurations are applied in argument order, so later operations can override earlier ones.  
- With no arguments, `Configr.Combine()` has no effect.  

**Exceptions:**  
- `ArgumentNullException`: When the provided configuration array is null.  
- `ArgumentNullException`: When a configuration in the array is null.  
