# DateTimes
Use `Fuzzr.DateTime()`.  
- **Default:** min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)) inclusive, snapped to whole seconds.  
- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a `DateTime` in the inclusive range [min, max], snapped to whole seconds.  
