# NeverReturnNull
Filters out nulls from a nullable Fuzzr, retrying up to the retry limit.  

**Signature:**  
```csharp
ExtFuzzr.NeverReturnNull<T>(this FuzzrOf<T?> fuzzr)
```
  

**Exceptions:**  
- `NonNullValueExhaustedException`: When all attempts result in null.  
