# Configr.Ignore(Func&lt;PropertyInfo, bool&gt; predicate)
Skips all properties matching the predicate across all types during generation.  
Use to exclude recurring patterns like identifiers, foreign keys, or audit fields.  
**Usage:**  
```csharp
from ignore in Configr.Ignore(a => a.Name == "Name")
from person in Fuzzr.One<Person>()
from fileEntry in Fuzzr.One<FileEntry>()
select (person, fileEntry);
// Results in => 
// ( Person { Name: "", Age: 67 }, FileEntry { Name: "" } )
```
