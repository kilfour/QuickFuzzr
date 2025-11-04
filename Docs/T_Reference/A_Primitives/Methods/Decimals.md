# Decimals
Use `Fuzzr.Decimal()`.  
- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = 1, max = 100).  
- Can be made to return `decimal?` using the `.Nullable()` combinator.  
- `decimal` is automatically detected and generated for object properties.  
- `decimal?` is automatically detected and generated for object properties.  
