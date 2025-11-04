# `Configr&lt;T&gt;.Construct(...)
**Usage:**  
```csharp
 Configr<SomeThing>.Construct(Fuzzr.Constant(42));
```
Subsequent calls to `Fuzzr.One<T>()` will then use the registered constructor.  
Various overloads exist that allow for up to five constructor arguments.  

After that, ... you're on your own.  
