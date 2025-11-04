# DateTimes
Use `Fuzzr.DateTime()`.  
- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a DateTime greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).  
- Can be made to return `DateTime?` using the `.Nullable()` combinator.  
- `DateTime` is automatically detected and generated for object properties.  
- `DateTime?` is automatically detected and generated for object properties.  
