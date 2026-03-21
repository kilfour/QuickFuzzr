# ToList
Creates a Fuzzr that materializes the sequence produced by the source Fuzzr as a `List<T>`.  
This is a convenience method.
The equivalent behavior can be expressed with LINQ Select, but it removes boilerplate.  

**Signature:**  
```csharp
ExtFuzzr.ToList(this FuzzrOf<T> fuzzr)
```
  

**Usage:**  
```csharp
Fuzzr.Int().Many(5).ToList();
```
