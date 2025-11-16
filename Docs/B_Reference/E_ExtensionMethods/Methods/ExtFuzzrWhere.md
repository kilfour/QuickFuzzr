# Where

**Signature:**  
```csharp
ExtFuzzr.Where(this FuzzrOf<T> fuzzr, Func<T,bool> predicate)
```
  
Filters generated values to those satisfying the predicate.  

**Exceptions:**  
- `PredicateUnsatisfiedException`: When no value satisfies the predicate within the retry limit.  
- `ArgumentNullException`: When the predicate is `null`.  
