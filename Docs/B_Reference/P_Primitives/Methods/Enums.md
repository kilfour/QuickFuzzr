# Enums
Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.  
> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.  
- The default Fuzzr just picks a random value from all enumeration values.  
- Passing in a non Enum type for T throws an ArgumentException.  
