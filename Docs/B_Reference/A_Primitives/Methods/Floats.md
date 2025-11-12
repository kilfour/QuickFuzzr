# Floats
Use `Fuzzr.Float()`.  
- The overload `Fuzzr.Float(float min, float max)` generates a float greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = 1, max = 100).  
- Can be made to return `float?` using the `.Nullable()` combinator.  
- `float` is automatically detected and generated for object properties.  
- `float?` is automatically detected and generated for object properties.  
