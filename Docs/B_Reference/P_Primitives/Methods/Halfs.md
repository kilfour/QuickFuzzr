# Halfs
Use `Fuzzr.Half()`.  

- The overload Fuzzr.Half(Half min, Half max) generates a half-precision floating-point number greater than or equal to `min` and less than `max`.
  *Note:* Due to floating-point rounding, max may occasionally be produced.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr is (min = (Half)1, max = (Half)100).  
