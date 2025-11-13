# Chars
Use `Fuzzr.Char()`.  
- The overload `Fuzzr.Char(char min, char max)` generates a char greater than or equal to `min` and less than or equal to `maxs`.  
- Throws an `ArgumentException` when `min` > `max`.  
- The default fuzzr always generates a char between lower case 'a' and lower case 'z'.  
- Can be made to return `char?` using the `.Nullable()` combinator.  
- `char` is automatically detected and generated for object properties.  
- `char?` is automatically detected and generated for object properties.  
