# Shuffle
Creates a Fuzzr that produces a random permutation of the provided sequence.  
Use for randomized ordering, unbiased sampling without replacement.
  

**Signature:**  
```csharp
Fuzzr.Shuffle(params T[] values)
```
  

**Usage:**  
```csharp
Fuzzr.Shuffle("John", "Paul", "George", "Ringo");
// Results in => ["Paul", "Ringo", "John", "George"]
```
- If the input sequence is empty, the result is also empty.  

**Overloads:**  
- `Shuffle<T>(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  

**Exceptions:**  
  - `ArgumentNullException`: When the input collection is `null`.  
