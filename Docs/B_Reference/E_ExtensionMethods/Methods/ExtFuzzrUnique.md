# Unique
Makes sure that every generated value is unique.  

**Signature:**  
```csharp
ExtFuzzr.Unique<T>(this FuzzrOf<T> fuzzr, object key)
```
  
- Multiple unique Fuzzrs can be defined in one 'composed' Fuzzr, without interfering with eachother by using a different key.  
- When using the same key for multiple unique Fuzzrs all values across these Fuzzrs are unique.  

**Overloads:**  
- `Unique<T>(this FuzzrOf<T> fuzzr, object key, int maxAttempts)`  
  Overwrites the global retry limit with the provided value.  
- `Unique<T>(this FuzzrOf<T> fuzzr, Func<object> key)`  
  Takes a function as an argument allowing for a dynamic key.  

**Overloads:**  
- `Unique<T>(this FuzzrOf<T> fuzzr, Func<object> key, int maxAttempts)`  
  Overwrites the global retry limit with the provided value.  

**Exceptions:**  
- `UniqueValueExhaustedException`: When the Fuzzr cannot find enough unique values within the retry limit.   
- `NullReferenceException`: When the provided `Fuzzr` is null.  
