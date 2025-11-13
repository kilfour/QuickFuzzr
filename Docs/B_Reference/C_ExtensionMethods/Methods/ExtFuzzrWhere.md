# Where(this FuzzrOf&lt;T&gt; fuzzr, Func&lt;T,bool&gt; predicate)
Filters generated values to those satisfying the predicate.  

**Exceptions:**  
- `PredicateUnsatisfiedException`: When no value satisfies the predicate within the retry limit.  
