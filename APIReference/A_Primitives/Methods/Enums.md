# Enums
Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.  
> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.  
- The default generator just picks a random value from all enumeration values.  
- An Enumeration is automatically detected and generated for object properties.  
- A nullable enumeration is automatically detected and generated for object properties.  
- Passing in a non Enum type for T throws an ArgumentException.  
