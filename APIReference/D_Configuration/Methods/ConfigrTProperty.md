# Configr&lt;T&gt;.Property(...)
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
