# DateTimes
Use `Fuzzr.DateTime()`.  
- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a `DateTime` in the inclusive range [min, max], snapped to whole seconds.  
- Generated values are snapped to whole seconds.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)) inclusive, snapped to whole seconds.  
