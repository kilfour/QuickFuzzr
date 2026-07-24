# A Heap of Trouble
A min-heap is a binary tree where every node's value is less than or equal to
the values of its children.

The children's valid value range depends on the value generated for their
parent. LINQ composition makes that dependency explicit.  
```csharp
public record HeapNode(int Value, HeapNode? Left, HeapNode? Right);
```
The recursive generator receives the remaining `depth` and the current `minimum` value:  
```csharp
if (depth == 0)
    return Fuzzr.Constant<HeapNode?>(null);
return
    from value in Fuzzr.Int(minimum, 101)
    from left in HeapFuzzr(depth - 1, value)
    from right in HeapFuzzr(depth - 1, value)
    select new HeapNode(value, left, right);
```
`HeapFuzzr` first chooses the current value, then uses it as the lower bound
while recursively generating both children. The depth argument makes the
result a complete binary tree and gives the recursion an explicit stopping
point.  
Generate heaps of varying depths:  
```csharp
    from depth in Fuzzr.Int(2, 5)
    from heap in HeapFuzzr(depth, 1)
    select heap!;
```
