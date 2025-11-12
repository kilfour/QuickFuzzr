# Configr&lt;T&gt;.Ignore(...)
**Usage:**  
```csharp
from ignore in Configr<Person>.Ignore(a => a.Name)
from person in Fuzzr.One<Person>()
select person;
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
The property specified will be ignored during generation.  
Derived classes generated also ignore the base property.  
