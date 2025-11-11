# Fuzzr.OneOf&lt;T&gt;(params &lt;T&gt;[] values)
Creates a generator that randomly selects one value or generator from the provided options.  
**Usage:**  
```csharp
 Fuzzr.OneOf("a", "b", "c");
```
- Selection is uniform unless weights are specified (see below).  

**Overloads:**  
- `Fuzzr.OneOf(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  
- `Fuzzr.OneOf(params FuzzrOf<T>[] generators)`:  
  Randomly selects and executes one of the provided generators.  
- `Fuzzr.OneOf(params (int Weight, T Value)[] weightedValues)`:  
  Selects a value using weighted probability. The higher the weight, the more likely the value is to be chosen.  
- `Fuzzr.OneOf(params (int Weight, FuzzrOf<T> Generator)[] weightedGenerators)`:  
  Like the weighted values overload, but applies weights to generators.  

**Exceptions:**  
  - `OneOfEmptyOptionsException`: When trying to choose from an empty collection.  
  - `NegativeWeightException`: When one or more weights are negative.  
  - `ZeroTotalWeightException`: When the total of all weights is zero or negative.  
