# ULongs
Use `Fuzzr.ULong()`.  
- The overload `Fuzzr.ULong(ulong min, ulong max)` generates a ulong greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = 1, max = 100).  
