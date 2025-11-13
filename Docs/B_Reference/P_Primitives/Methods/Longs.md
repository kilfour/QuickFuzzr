# Longs
Use `Fuzzr.Long()`.  
- The overload `Fuzzr.Long(long min, long max)` generates a long greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
