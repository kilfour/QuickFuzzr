# Fuzzr.Counter(object key)
This generator returns an `int` starting at 1, and incrementing by 1 on each call.  
Useful for generating unique sequential IDs or counters.  
  
**Usage:**  
```csharp
Fuzzr.Counter("the-key").Many(5).Generate();
// Returns => [1, 2, 3, 4, 5]
```
- Each `key` maintains its own independent counter sequence.  
- Counter state resets between separate `Generate()` calls.  
- Works seamlessly in LINQ chains and with .Apply(...) to offset or transform the sequence.  
