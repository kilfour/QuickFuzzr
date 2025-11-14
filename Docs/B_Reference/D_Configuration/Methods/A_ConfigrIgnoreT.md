# Configr&lt;T&gt;.Ignore
The property specified will be ignored during generation.  

**Signature:**  
```csharp
Configr<T>.Ignore(Expression<Func<T, TProperty>> expr)
```
  

**Usage:**  
```csharp
from ignore in Configr<Person>.Ignore(a => a.Name)
from person in Fuzzr.One<Person>()
select person;
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
Derived classes generated also ignore the base property.  
