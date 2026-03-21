# Apply
Executes a side-effect per generated value without altering it.  

**Signature:**  
```csharp
ExtFuzzr.Apply(this FuzzrOf<T> fuzzr, Action<T> action)
```
  

**Usage:**  
```csharp
var seen = new List<int>();
var fuzzr = Fuzzr.Int().Apply(seen.Add);
var value = fuzzr.Generate();
// seen now equals [ 67 ]
```

**Exceptions:**  
- `ArgumentNullException`: When the provided `Action` is null.  
