# Configr&lt;T&gt;.Property
The property specified will be generated using the passed in Fuzzr.  

**Signature:**  
```csharp
Configr<T>.Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> fuzzr)
```
  

**Usage:**  
```csharp
 Configr<Person>.Property(s => s.Age, Fuzzr.Constant(42));
```
- Derived classes generated also use the custom property.  

**Overloads:**  
- `Configr<T>.Property<TProperty>(Func<PropertyInfo, bool> predicate, TProperty value)`  
  Allows for passing a value instead of a Fuzzr.  

**Exceptions:**  
- `ArgumentNullException`: When the expression is `null`.  
- `ArgumentNullException`: When the Fuzzr is `null`.  
- `PropertyConfigurationException`: When the expression points to a field instead of a property.  
