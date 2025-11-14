# Configr&lt;T&gt;.With

**Signature:**  
```csharp
Configr<T>.With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)
```
  
Applies configuration for type `T` based on a value produced by another fuzzr,
allowing dynamic, data-dependent configuration inside LINQ chains.
  
