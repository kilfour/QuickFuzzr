# Unique
Makes sure that every generated value is unique.  

**Signature:**  
```csharp
ExtFuzzr.Unique<T>(this FuzzrOf<T> fuzzr, object key)
```
  
- When asking for more unique values than the fuzzr can supply, an exception is thrown.  
- Multiple unique fuzzrs can be defined in one 'composed' fuzzr, without interfering with eachother by using a different key.  
- When using the same key for multiple unique fuzzrs all values across these fuzzrs are unique.  
- An overload exist taking a function as an argument allowing for a dynamic key.  
