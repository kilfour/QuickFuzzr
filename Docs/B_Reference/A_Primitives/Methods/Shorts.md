# Shorts
Use `Fuzzr.Short()`.  
- The overload `Fuzzr.Short(short min, short max)` generates a short greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = 1, max = 100).  
- Can be made to return `short?` using the `.Nullable()` combinator.  
- `short` is automatically detected and generated for object properties.  
- `short?` is automatically detected and generated for object properties.  
