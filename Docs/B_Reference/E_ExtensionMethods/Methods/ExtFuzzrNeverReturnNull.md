# NeverReturnNull
Filters out nulls from a nullable fuzzr, retrying up to the retry limit.  

**Signature:**  
```csharp
ExtFuzzr.NeverReturnNull<T>(this FuzzrOf<T?> fuzzr)0
```
  

**Exceptions:**  
- `NonNullValueExhaustedException`: When all attempts result in null.  
