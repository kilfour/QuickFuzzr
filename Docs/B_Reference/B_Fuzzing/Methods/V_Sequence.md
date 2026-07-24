# Sequence
Creates a Fuzzr that produces each provided value in order.  
After the final value, it starts again at the beginning.
  

**Signature:**  
```csharp
Fuzzr.Sequence(params T[] values)
```
  

**Usage:**  
```csharp
Fuzzr.Sequence(42, 43, 44);
// Generate once results in => 42
// second generate => 43
// third generate => 44
// fourth generate => 42
// ...
```
- Sequence state resets between separate `Generate()` calls.  
- The provided values are captured when the Fuzzr is created.  

**Exceptions:**  
- `ArgumentNullException`: When the provided values are null.  
- `ArgumentException`: When no values are provided.  
