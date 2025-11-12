# Cooking Up a Fuzzr
## Contents

- [A Deep Dark Forest][ADeepDarkForest]
  

[ADeepDarkForest]: #a-deep-dark-forest
## A Deep Dark Forest
Using `Fuzzr<T>.Depth()` together with the `Fuzzr<T>.AsOneOf(...)` combinator and `Fuzzr<T>.EndOn<TEnd>()`
allows you to build tree type hierarchies.  
Given the canonical abstract `Tree`, concrete `Branch` and `Leaf` example model:   
```csharp
public abstract class Tree { }
public class Leaf : Tree { }
public class Branch : Tree
{
    public Tree? Left { get; set; }
    public Tree? Right { get; set; }
}
```
We can generate a forest like so:  
```csharp
    from depth in Configr<Tree>.Depth(2, 5)
    from inheritance in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
    from terminator in Configr<Tree>.EndOn<Leaf>()
    from tree in Fuzzr.One<Tree>()
    select tree;
```
**Output:**  
```text
Branch
  ├── Branch
  │   ├── Leaf
  │   └── Branch
  │       ├── Leaf
  │       └── Leaf
  └── Leaf
```
