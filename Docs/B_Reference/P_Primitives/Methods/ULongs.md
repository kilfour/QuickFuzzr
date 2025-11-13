# ULongs
Use `Fuzzr.ULong()`.  
- The overload `Fuzzr.ULong(ulong min, ulong max)` generates a ulong greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = 1, max = 100).  
- Can be made to return `ulong?` using the `.Nullable()` combinator.  
- `ulong` is automatically detected and generated for object properties.  
- `ulong?` is automatically detected and generated for object properties.  
