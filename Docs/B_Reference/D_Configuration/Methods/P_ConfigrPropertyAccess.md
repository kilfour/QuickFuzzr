# Property Access
Control which kinds of properties QuickFuzzr is allowed to populate.  

**Signature:**
```csharp
Configr.EnablePropertyAccessFor(PropertyAccess propertyAccess) 
Configr.DisablePropertyAccessFor(PropertyAccess propertyAccess)
```  

**Usage:**  
```csharp
from enable in Configr.EnablePropertyAccessFor(PropertyAccess.InitOnly)
from person1 in Fuzzr.One<PrivatePerson>()
from disable in Configr.DisablePropertyAccessFor(PropertyAccess.InitOnly)
from person2 in Fuzzr.One<PrivatePerson>()
select (person1, person2);
// Results in => ( { Name: "xiyi", Age: 94 }, { Name: "", Age: 0 } )
```
- Updates state flags using bitwise enable/disable semantics.  
- The default value is `PropertyAccess.PublicSetters`.  
- `ReadOnly` only applies to get-only **auto-properties**.  
- Getter-only properties *without* a compiler-generated backing field (i.e.: calculated or manually-backed) are never auto-generated.  
