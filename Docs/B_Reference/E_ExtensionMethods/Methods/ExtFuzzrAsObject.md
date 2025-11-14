# AsObject
Boxes generated values as object without altering them.  

**Signature:**  
```csharp
ExtFuzzr.AsObject<T>(this FuzzrOf<T> fuzzr)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(42).AsObject();
// Results in => 42
```
