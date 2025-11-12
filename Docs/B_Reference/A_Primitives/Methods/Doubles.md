# Doubles
Use `Fuzzr.Double()`.  
- The overload `Fuzzr.Double(double min, double max)` generates a double greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `double?` using the `.Nullable()` combinator.  
- `double` is automatically detected and generated for object properties.  
- `double?` is automatically detected and generated for object properties.  
