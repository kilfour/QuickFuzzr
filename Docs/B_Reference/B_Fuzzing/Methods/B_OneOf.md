# OneOf
Creates a fuzzr that randomly selects one value or fuzzr from the provided options.  

**Signature:**  
```csharp
Fuzzr.OneOf(params T[] values)
```
  

**Usage:**  
```csharp
 Fuzzr.OneOf("a", "b", "c");
```
- Selection is uniform unless weights are specified (see below).  
- **null items** are allowed in `params T[] values`.  

**Overloads:**  
- `Fuzzr.OneOf(IEnumerable<T> values)`:  
  Same as above, but accepts any enumerable source.  
- `Fuzzr.OneOf(params FuzzrOf<T>[] fuzzrs)`:  
  Randomly selects and executes one of the provided fuzzrs.  
- `Fuzzr.OneOf(params (int Weight, T Value)[] weightedValues)`:  
  Selects a value using weighted probability. The higher the weight, the more likely the value is to be chosen.  
```csharp
 Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c"));
```
- `Fuzzr.OneOf(params (int Weight, FuzzrOf<T> fuzzr)[] weightedFuzzrs)`:  
  Like the weighted values overload, but applies weights to fuzzrs.  

**Exceptions:**  
  - `OneOfEmptyOptionsException`: When trying to choose from an empty collection.  
  - `NegativeWeightException`: When one or more weights are negative.  
  - `ZeroTotalWeightException`: When the total of all weights is zero or negative.  
  - `ArgumentNullException`: When the provided sequence is null.  
