# Bytes
Use `Fuzzr.Byte()`.  
- The default generator produces a `byte` in the full range (`0`-`255`).  
- The overload `Fuzzr.Byte(int min, int max)` generates a value greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- Throws an `ArgumentOutOfRangeException` when `min` < `byte.MinValue` (i.e. `< 0`).  
- Throws an `ArgumentOutOfRangeException` when `max` > `byte.MaxValue` (i.e. `> 255`).  
- When `min == max`, the generator always returns that exact value.  
- Boundary coverage: over time, values at both ends of the interval should appear.  
- Can be made to return `byte?` using the `.Nullable()` combinator.  
- `byte` is automatically detected and generated for object properties.  
- `byte?` is automatically detected and generated for object properties.  
