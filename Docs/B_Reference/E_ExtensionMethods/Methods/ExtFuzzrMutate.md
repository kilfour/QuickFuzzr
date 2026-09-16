# Mutate
Derives a new Fuzzr by transforming an existing Fuzzr. For each generated value, one mutation strategy is chosen and repeated, strategies are alternatives, not a pipeline. A mutation transforms a FuzzrOf<T> into another FuzzrOf<T>, so it can use Select, LINQ, or other generator extensions.  

**Signature:**  
```csharp
fuzzr.Mutate((mutation, times), ...)
```
  

**Signature:**  
```csharp
fuzzr.Mutate((mutation, min, max), ...)
```
  
Supply mutations as (mutation, times) tuples for fixed repetition or (mutation, min, max) tuples for ranged repetition. Each mutation is a function from FuzzrOf<T> to FuzzrOf<T>.  

**Usage:**  
```csharp
return Fuzzr.Constant(10).Mutate((source => source.Select(value => value + 1), 1));
// Always generates 11: the transformation is applied once.
```
A (mutation, times) tuple repeats that transformation exactly times times. Repetition composes transformations; it does not produce a collection of independently mutated values.  
```csharp
return Fuzzr.Constant(10).Mutate(
    (source => source.Select(value => value + 1), 3));
// Always generates 13: the same transformation is applied three times.
```
A (mutation, min, max) tuple draws a repetition count for each generated value. Bounds follow Fuzzr.Int: min is inclusive and max is exclusive; equal bounds give that exact count. Use nonnegative counts. A fixed count of zero leaves the source unchanged.  
```csharp
return Fuzzr.Constant(10).Mutate(
    (source => source.Select(value => value + 1), 1, 4));
// Generates 11, 12, or 13: Min is inclusive; Max is exclusive.
```
When several mutations are supplied, each generated value uses one chosen strategy for all its repetitions. To compose different strategies in sequence, chain separate Mutate calls.  
```csharp
return Fuzzr.Constant("x").Mutate(
    (source => source.Select(text => text + "!"), 2),
    (source => source.Select(text => text + "?"), 2));
// Generates "x!!" or "x??", never "x!?" or "x?!".
```
Mutations can introduce variations around a known starting value. They do not guarantee invalid input, or even a changed value: those properties depend on the transformation and the domain. In this example insertion guarantees one added character, but whether whitespace is allowed belongs to the consuming test.  
```csharp
return Fuzzr.Constant("#A1B2C3").Mutate(
    (source => source.InsertOneOf(' ', '\t'), 1));
// Adds one space or tab at a generated position, including either end.
// InsertOneOf requires using QuickFuzzr.Strings.
```
