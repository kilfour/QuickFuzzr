# NeverReturnNull(this FuzzrOf&lt;T?&gt; fuzzr)
Filters out nulls from a nullable fuzzr, retrying up to the retry limit.  

**Usage:**  

**Exceptions:**  
- `NonNullValueExhaustedException`: When all attempts result in null.  
