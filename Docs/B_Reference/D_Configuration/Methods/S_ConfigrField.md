# Configr.Field
Any public field matching the predicate uses the configured Fuzzr.  

**Signature:**  
```csharp
Configr.Field<TField>(Func<FieldInfo, bool> predicate, FuzzrOf<TField> fuzzr)
```
  

**Usage:**  
```csharp
 Configr.Field(field => field.Name == "Age", Fuzzr.Constant(42));
```

**Overloads:**  
- `Configr.Field<TField>(Func<FieldInfo, bool> predicate, TField value)`  
- `Configr.Field<TField>(Func<FieldInfo, bool> predicate, Func<FieldInfo, FuzzrOf<TField>> factory)`  
- `Configr.Field<TField>(Func<FieldInfo, bool> predicate, Func<FieldInfo, TField> factory)`  

**Exceptions:**  
- `ArgumentNullException`: When the predicate, Fuzzr, or factory is `null`.  
