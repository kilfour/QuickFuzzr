# Fuzzing
This section lists the core Fuzzrs responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a `FuzzrOf<T>` and can be composed using standard LINQ syntax.  
## Contents
| Fuzzr| Description |
| -| - |
| [One](Methods/A_One.md)| Creates a Fuzzr that produces an instances of type `T`. |
| [OneOf](Methods/B_OneOf.md)| Randomly selects one of the provided values. |
| [From Each](Methods/C_FromEach.md)| Creates a Fuzzr that produces an IEnumerable based on the elements of a source IEnumerable. |
| [Shuffle](Methods/D_Shuffle.md)| Creates a Fuzzr that randomly shuffles an input sequence. |
| [Counter](Methods/E_Counter.md)| Generates a sequential integer per key, starting at 1. |
| [Constant](Methods/F_Constant.md)| Wraps a fixed value in a Fuzzr, producing the same result every time. |
