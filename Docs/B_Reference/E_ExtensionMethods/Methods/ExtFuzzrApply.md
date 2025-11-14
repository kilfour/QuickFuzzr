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

**Overloads:**  
- `Apply(this FuzzrOf<T> fuzzr, Func<T,T> func)`  
  Transforms generated values while preserving generation context.  
```csharp
Fuzzr.Constant(41).Apply(x => x + 1);
// Results in => 42
```

**Exceptions:**  
- `NullReferenceException`: When the provided `Action` or `Func` is null.  
