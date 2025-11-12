# Ints
Use `Fuzzr.Int()`.  
- The overload `Fuzzr.Int(int min, int max)` generates an int greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `int?` using the `.Nullable()` combinator.  
- `int` is automatically detected and generated for object properties.  
- `int?` is automatically detected and generated for object properties.  
