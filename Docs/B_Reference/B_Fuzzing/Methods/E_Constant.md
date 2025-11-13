# Constant
This fuzzr wraps the value provided of type `T` in a `FuzzrOf<T>`.
It is most useful in combination with others and is often used to inject constants into combined fuzzrs.  

**Signature:**  
```csharp
Fuzzr.Constant(T value)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(41);
// Results in => 42
```
