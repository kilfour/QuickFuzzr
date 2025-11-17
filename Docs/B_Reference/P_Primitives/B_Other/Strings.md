# Strings
Use `Fuzzr.String()`.  
- The Default Fuzzr generates a string of length greater than or equal to 1 and less than or equal to 10.  
- The overload `Fuzzr.String(int min, int max)` generates a string of length greater than or equal to `min` and less than or equal to `max`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The overload `Fuzzr.String(int length)` generates a string of exactly `length` ... erm ... length.  
- Throws an `ArgumentOutOfRangeException` when `length` < 0.  
- The default Fuzzr always generates every char element of the string to be between lower case 'a' and lower case 'z'.  
- A version exists for all methods mentioned above that takes a `FuzzrOf<char>` as parameter and then this one will be used to build up the resulting string.  
