# Bytes
Use `Fuzzr.Byte()`.  
- The default Fuzzr produces a `byte` in the full range (`0`-`255`).  

**Overloads:**  
- `Fuzzr.Byte(int min, int max)`  
  Generates a value greater than or equal to `min` and less than or equal to `max`.  
  Boundary coverage: over time, values at both ends of the interval should appear.  

**Exceptions:**  
- `ArgumentOutOfRangeException`: When `min` > `max`.  
