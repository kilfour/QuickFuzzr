# Configr&lt;T&gt;.Depth(int min, int max)
Configures depth constraints for type `T` to control recursive object graph generation. 
  
**Usage:**  
```csharp
Configr<Turtle>.Depth(2, 5);
// Results in =>
// Turtle { Down: { Down: { Down: { Down: null } } } }
```
Subsequent calls to `Fuzzr.One<T>()` will generate between 2 and 5 nested levels of `Turtle` instances,
depending on the random draw and available recursion budget.  
Depth is per type, not global. Each recursive type manages its own budget.
  

**Exceptions:**  
- `ArgumentOutOfRangeException`: When min is negative.  

**Exceptions:**  
- `ArgumentOutOfRangeException`: When max is lesser than min  
