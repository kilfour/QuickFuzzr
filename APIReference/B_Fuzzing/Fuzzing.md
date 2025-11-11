# Fuzzing
This section lists the core generators responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a FuzzrOf<T> and can be composed using standard LINQ syntax.  
## Contents
| Fuzzr| Description |
| -| - |
| [Fuzzr.Constant&lt;T&gt;(T value)](Methods/FuzzrConstant.md)|   |
| [Fuzzr.Counter(object key)](Methods/FuzzrCounter.md)|   |
| [Fuzzr.One&lt;T&gt;()](Methods/FuzzrOne.md)|   |
| [Fuzzr.OneOf&lt;T&gt;(params &lt;T&gt;[] values)](Methods/FuzzrOneOf.md)|   |
| [Fuzzr.Shuffle&lt;T&gt;()](Methods/FuzzrShuffle.md)|   |
