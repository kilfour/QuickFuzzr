# UShorts
Use `Fuzzr.UShort()`.  
- The overload `Fuzzr.UShort(ushort min, ushort max)` generates a ushort greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `ushort?` using the `.Nullable()` combinator.  
- `ushort` is automatically detected and generated for object properties.  
- `ushort?` is automatically detected and generated for object properties.  
