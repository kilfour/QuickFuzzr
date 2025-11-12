# UInts
Use `Fuzzr.UInt()`.  
- The overload `Fuzzr.UInt(uint min, uint max)` generates an uint greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = 1, max = 100).  
- Can be made to return `uint?` using the `.Nullable()` combinator.  
- `uint` is automatically detected and generated for object properties.  
- `uint?` is automatically detected and generated for object properties.  
