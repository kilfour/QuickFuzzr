# DateOnlys
Use `Fuzzr.DateOnly()`.  
- The overload `Fuzzr.DateOnly(DateOnly min, DateOnly max)` generates a DateOnly greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = new DateOnly(1970, 1, 1), max = new DateOnly(2020, 12, 31)).  
