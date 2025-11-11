# Configr&lt;T&gt;.Property(...)
**Usage:**  
```csharp
 Configr<Thing>.Property(s => s.Id, Fuzzr.Constant(42));
```
- The property specified will be generated using the passed in generator.  
- An overload exists which allows for passing a value instead of a generator.  
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
• Use a property selector (e.g. a => a.PropertyName).
• Then pass it to Configr<PersonOutInTheFields>.Property(...) to configure generation.
```
