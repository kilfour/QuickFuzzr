# Floats
Use `Fuzzr.Float()`.  
- The overload `Fuzzr.Float(float min, float max)` generates a float greater than or equal to `min` and less than `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100).  
