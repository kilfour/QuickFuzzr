# Bytes
Use `Fuzzr.Byte()`.  
- The default Fuzzr produces a `byte` in the full range (`0`-`255`).  

**Overloads:**  
- `Fuzzr.Byte(int min, int max)`  
  Generates a value greater than or equal to `min` and less than or equal to `max`.  
  When `min == max`, the Fuzzr always returns that exact value.  
  Boundary coverage: over time, values at both ends of the interval should appear.  

**Exceptions:**  
- `ArgumentOutOfRangeException`: When `min` > `max`.  
- `ArgumentOutOfRangeException`: When `min` < `byte.MinValue` (i.e. `< 0`).  
- `ArgumentOutOfRangeException`: When `max` > `byte.MaxValue` (i.e. `> 255`).  
