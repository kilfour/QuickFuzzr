# Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) / Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)
**Usage:**  
```csharp
from enable in Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)
from person1 in Fuzzr.One<PrivatePerson>()
from disable in Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)
from person2 in Fuzzr.One<PrivatePerson>()
select (person1, person2);
// Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
```
Enables and then disables generation for init-only properties.  
Updates state flags using bitwise enable/disable semantics.  
