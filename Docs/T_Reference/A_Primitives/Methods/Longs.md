# Longs
Use `Fuzzr.Long()`.  
- The overload `Fuzzr.Long(long min, long max)` generates a long greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `long?` using the `.Nullable()` combinator.  
- `long` is automatically detected and generated for object properties.  
- `long?` is automatically detected and generated for object properties.  
