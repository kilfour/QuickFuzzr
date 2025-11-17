# Decimals
Use `Fuzzr.Decimal()`.  
- **Default:** min = 1, max = 100, precision = 2).  
- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal in the range [min, max) (min inclusive, max exclusive).  
- The overload `Decimal(int precision)` generates a decimal with up to `precision` decimal places.  
- Throws an `ArgumentException` when `precision` < `0`.  
- The overload `Decimal(decimal min, decimal max, int precision)` generates a decimal in the range [min, max) (min inclusive, max exclusive), with up to `precision` decimal places.  
- Throws an `ArgumentException` when `precision` < `0`.  
