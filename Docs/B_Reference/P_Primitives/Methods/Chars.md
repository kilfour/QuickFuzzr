# Chars
Use `Fuzzr.Char()`.  
- The default Fuzzr always generates a char between lower case 'a' and lower case 'z'.  

**Overloads:**  
- `Fuzzr.Char(char min, char max)`  
 Generates a char greater than or equal to `min` and less than or equal to `max`.  
  When `min == max`, the Fuzzr always returns that exact value.  
  Boundary coverage: over time, values at both ends of the interval should appear.  

**Exceptions:**  
- `ArgumentOutOfRangeException`: When `min` > `max`.  
