# Chars
Use `Fuzzr.Char()`.  
- The overload `Fuzzr.Char(char min, char max)` generates a char greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default Fuzzr always generates a char between lower case 'a' and lower case 'z'.  
