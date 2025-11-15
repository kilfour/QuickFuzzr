# Constant
This Fuzzr wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined Fuzzrs.  

**Signature:**  
```csharp
Fuzzr.Constant(T value)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(42);
// Results in => 42
```
