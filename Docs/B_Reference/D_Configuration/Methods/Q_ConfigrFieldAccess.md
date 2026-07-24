# Configr.EnableFieldAccess
Field access is opt-in. By default, QuickFuzzr only populates properties.  

**Signature:**  
```csharp
Configr.EnableFieldAccess()
```
  

**Usage:**  
```csharp
    from _ in Configr.EnableFieldAccess()
    from person in Fuzzr.One<PersonOutInTheFields>()
    select person;
```
- Populates public instance fields.  
- Static, constant, readonly, and non-public fields are not populated.  
