# Decimals
Use `Fuzzr.Decimal()`.  

- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal greater than or equal to `min` and less than `max`.  
- The overload `Decimal(int precision)` generates a decimal with `precision` precision.  
- The overload `Decimal(decimal min, decimal max, int precision)` generates a decimal greater than or equal to `min` and less than `max`, with `precision` precision.  
- When `min == max`, the fuzzr always returns that exact value.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = 1, max = 100, precision = 2).  
- Can be made to return `decimal?` using the `.Nullable()` combinator.  
- `decimal` is automatically detected and generated for object properties.  
- `decimal?` is automatically detected and generated for object properties.  
