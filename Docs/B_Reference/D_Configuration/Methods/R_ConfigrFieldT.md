# Configr&lt;T&gt;.Field
Explicitly configures a public field. This does not require `Configr.EnableFieldAccess()`.  

**Signature:**  
```csharp
Configr<T>.Field<TField>(Expression<Func<T, TField>> expression, FuzzrOf<TField> fuzzr)
```
  

**Usage:**  
```csharp
 Configr<PersonOutInTheFields>.Field(person => person.Age, Fuzzr.Constant(42));
```

**Overloads:**  
- `Configr<T>.Field<TField>(Expression<Func<T, TField>> expression, TField value)`  
  Allows for passing a value instead of a Fuzzr.  

**Exceptions:**  
- `FieldConfigurationException`: When the expression points to something other than a field.  
- `ArgumentNullException`: When the expression or Fuzzr is `null`.  
