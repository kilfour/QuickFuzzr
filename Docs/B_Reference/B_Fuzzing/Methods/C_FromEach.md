# From Each
Creates a Fuzzr that produces an IEnumerable based on the elements of a source IEnumerable.  

**Signature:**  
```csharp
FromEach<T, U>(IEnumerable<T> source, Func<T, FuzzrOf<U>> func)
```
  

**Usage:**  
```csharp
Fuzzr.FromEach([1, 100, 1000], a => Fuzzr.Int(1, a));
// Results in => [ 1, 67, 141 ]
```
