# ExtFuzzr.Where(this FuzzrOf&lt;T> fuzzr, Func&lt;T,bool> predicate)
Filters generated values to those satisfying the predicate.  

**Exceptions:**  
- `PredicateUnsatisfiedException`: When no value satisfies the predicate within the retry limit.  
