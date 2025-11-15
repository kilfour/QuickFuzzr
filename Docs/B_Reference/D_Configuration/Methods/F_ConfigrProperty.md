# Configr.Property
Any property matching the predicate will use the specified Fuzzr during generation.  

**Signature:**  
```csharp
Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)
```
  

**Usage:**  
```csharp
 Configr.Property(a => a.Name == "Id", Fuzzr.Constant(42));
```

**Overloads:**  
- `Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)`  
  Allows you to pass in a value instead of a Fuzzr.  
- `Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, Func<PropertyInfo, FuzzrOf<TProperty>> factory)`  
  Allows you to create a Fuzzr dynamically using a factory method.  
- `Configr.Property<TProperty>(Func<PropertyInfo, bool> predicate, Func<PropertyInfo, TProperty> factory)`  
  Allows you to create a value dynamically using a factory method.  

**Exceptions:**  
- `ArgumentNullException`: When the predicate is `null`.  
- `ArgumentNullException`: When the Fuzzr is `null`.  
- `ArgumentNullException`: When the factory function is `null`.  
