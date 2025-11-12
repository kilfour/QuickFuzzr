# Guids
Use `Fuzzr.Guid()`. *There is no overload.*  
- The default fuzzr never generates Guid.Empty.  
- `Fuzzr.Guid()` is deterministic when seeded.  
- Can be made to return `Guid?` using the `.Nullable()` combinator.  
- `Guid` is automatically detected and generated for object properties.  
- `Guid?` is automatically detected and generated for object properties.  
