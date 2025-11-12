# ExtFuzzr.NeverReturnNull(this FuzzrOf&lt;T?> fuzzr)
Filters out nulls from a nullable fuzzr, retrying up to the retry limit.  
**Usage:**  

**Exceptions:**  
- `NonNullValueExhaustedException`: When all attempts result in null.  
