# Configr<T>.With
Applies configuration for type `T` based on a value generated from another fuzzr.  

**Signature:**  
```csharp
Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)
```
  
