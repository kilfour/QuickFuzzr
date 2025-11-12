# Halfs
Use `Fuzzr.Half()`.  
- The overload Fuzzr.Half(Half min, Half max) generates a half-precision floating-point number greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = (Half)1, max = (Half)100).  
- Can be made to return `Half?` using the `.Nullable()` combinator.  
- `Half` is automatically detected and generated for object properties.  
- `Half?` is automatically detected and generated for object properties.  
