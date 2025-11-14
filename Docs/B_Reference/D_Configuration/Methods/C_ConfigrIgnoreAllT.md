# Configr&lt;T&gt;.IgnoreAll
Ignore all properties while generating an object.  

**Signature:**  
```csharp
Configr<T>.IgnoreAll()
```
  

**Usage:**  
```csharp
from ignore in Configr<Person>.IgnoreAll()
from person in Fuzzr.One<Person>()
from address in Fuzzr.One<Address>()
select (person, address);
// Results in => 
// ( Person { Name: "", Age: 0 }, Address { Street: "", City: "" } )
```
`IgnoreAll()`does not cause properties on derived classes to be ignored, even inherited properties.  
