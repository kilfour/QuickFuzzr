# Cooking Up a Fuzzr
## Contents

- [A Deep Dark Forest][ADeepDarkForest]
- [Sometimes the Cheetah Needs to Run][SometimesTheCheetahNeedsToRun]
  

[ADeepDarkForest]: #a-deep-dark-forest

[SometimesTheCheetahNeedsToRun]: #sometimes-the-cheetah-needs-to-run
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
## Sometimes the Cheetah Needs to Run
Most of the time, QuickFuzzr is fast enough.  
Sure there's faster out there, and nothing beats a hand-rolled generator, but QuickFuzzr lands comfortably somewhere in the middle.  

And that's fine: it's built for **agility**, not raw speed.

But then again... *sometimes the cheetah needs to run*.
  
### Example: The Forest of a Thousand Trees
We could reuse the tree fuzzr from *"A Deep Dark Forest"* and simply do:  
```csharp
treefuzzr.Many(1000).Generate();
```
But since the configuration doesn't change between runs, 
we can optimize the `LINQ` chain a bit by **preloading** the config:  
```csharp
var treeConfigr =
    from depth in Configr<Tree>.Depth(2, 5)
    from inheritance in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
    from terminator in Configr<Tree>.EndOn<Leaf>()
    select Intent.Fixed;
```
Now we build the forest with the configuration already fixed in place:  
```csharp
var forestFuzzr =
    from cfg in treeConfigr
    from trees in Fuzzr.One<Tree>().Many(1000)
    select trees;
forestFuzzr.Generate();
```
**Benchmarks:**  
```markdown
| Method               | Mean     | Error     | StdDev    | Gen0     | Gen1     | Allocated |
|--------------------- |---------:|----------:|----------:|---------:|---------:|----------:|
| ConfigFuzzr          | 3.836 ms | 0.0579 ms | 0.0542 ms | 828.1250 | 312.5000 |   6.61 MB |
| PreloadedConfigFuzzr | 2.999 ms | 0.0348 ms | 0.0291 ms | 597.6563 | 195.3125 |   4.78 MB |
```

Not bad, considering this example tree model has no properties at all.

For more complex types, where property customization is heavier, the gains are noticeably larger.  
### Summary:

QuickFuzzr's dynamic configuration is usually fast enough, and you rarely need to optimize.  
But when you do: **lifting Configr calls out of the hot path** moves QuickFuzzr into the upper end of the performance spectrum.
  
