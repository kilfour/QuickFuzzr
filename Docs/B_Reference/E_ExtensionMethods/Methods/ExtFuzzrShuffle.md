# Shuffle
Randomly shuffles the sequence produced by the source Fuzzr.  

**Signature:**  
```csharp
ExFuzzr.Shuffle<T>(this FuzzrOf<IEnumerable<T>> source)
```
  

**Usage:**  
```csharp
Fuzzr.Counter("num").Many(4).Shuffle();
// Results in => [ 2, 4, 1, 3 ]
```
- Preserves the elements of the source sequence.  
