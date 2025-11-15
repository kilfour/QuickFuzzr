# Decimals
Use `Fuzzr.Decimal()`.  

- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal in the range [min, max) (min inclusive, max exclusive).  
- The overload `Decimal(int precision)` generates a decimal with up to `precision` decimal places.  
- The overload `Decimal(decimal min, decimal max, int precision)` generates a decimal in the range [min, max) (min inclusive, max exclusive), with up to `precision` decimal places.  
- When `min == max`, the Fuzzr always returns that exact value.  
- Throws an `ArgumentException` when `min` > `max`.  
- **Default:** min = 1, max = 100, precision = 2).  
