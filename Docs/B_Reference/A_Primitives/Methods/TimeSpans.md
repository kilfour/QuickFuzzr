# TimeSpans
Use `Fuzzr.TimeSpan()`.  
- The overload `Fuzzr.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.  
- The default fuzzr is (max = 1000).  
- Can be made to return `TimeSpan?` using the `.Nullable()` combinator.  
- `TimeSpan` is automatically detected and generated for object properties.  
- `TimeSpan?` is automatically detected and generated for object properties.  
