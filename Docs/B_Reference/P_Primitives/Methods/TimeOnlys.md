# TimeOnlys
Use `Fuzzr.TimeOnly()`.  
- The overload `Fuzzr.TimeOnly(TimeOnly min, TimeOnly max)` generates a TimeOnly greater than or equal to `min` and less than `max`.  
- The default fuzzr is (min = 00:00:00, max = 23:59:59.9999999).  
- Can be made to return `TimeOnly?` using the `.Nullable()` combinator.  
- `TimeOnly` is automatically detected and generated for object properties.  
- `TimeOnly?` is automatically detected and generated for object properties.  
