# Fuzzing
This section lists the core fuzzrs responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a FuzzrOf<T> and can be composed using standard LINQ syntax.  
## Contents
| Fuzzr| Description |
| -| - |
| [Fuzzr.Constant&lt;T&gt;(T value)](Methods/FuzzrConstant.md)| Wraps a fixed value in a fuzzr, producing the same result every time. |
| [Fuzzr.Counter(object key)](Methods/FuzzrCounter.md)| Generates a sequential integer per key, starting at 1. |
| [Fuzzr.One&lt;T&gt;()](Methods/FuzzrOne.md)| Creates a fuzzr that produces an instances of type `T`. |
| [Fuzzr.OneOf&lt;T&gt;(params &lt;T&gt;[] values)](Methods/FuzzrOneOf.md)| Randomly selects one of the provided values. |
| [Fuzzr.Shuffle&lt;T&gt;()](Methods/FuzzrShuffle.md)| Creates a fuzzr that randomly shuffles an input sequence. |
