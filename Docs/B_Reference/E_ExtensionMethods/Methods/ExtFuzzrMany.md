# Many
Produces a fixed number of values from a fuzzr.  

**Signature:**  
```csharp
ExtFuzzr.Many(this FuzzrOf<T> fuzzr, int number)
```
  

**Usage:**  
```csharp
Fuzzr.Constant(6).Many(3);
// Results in => [6, 6, 6]
```

**Overloads:**  
- `Many(this FuzzrOf<T> fuzzr, int min, int max)`  
  Produces a variable number of values within bounds.  
