# Fuzzing
This section lists the core fuzzrs responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a `FuzzrOf<T>` and can be composed using standard LINQ syntax.  
## Contents
| Fuzzr| Description |
| -| - |
| [One](Methods/A_One.md)| Creates a fuzzr that produces an instances of type `T`. |
| [OneOf](Methods/B_OneOf.md)| Randomly selects one of the provided values. |
| [Shuffle](Methods/C_Shuffle.md)| Creates a fuzzr that randomly shuffles an input sequence. |
| [Counter](Methods/D_Counter.md)| Generates a sequential integer per key, starting at 1. |
| [Constant](Methods/E_Constant.md)| Wraps a fixed value in a fuzzr, producing the same result every time. |
