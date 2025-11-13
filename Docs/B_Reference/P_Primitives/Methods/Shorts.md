# Shorts
Use `Fuzzr.Short()`.  
- The overload `Fuzzr.Short(short min, short max)` generates a short greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = 1, max = 100).  
