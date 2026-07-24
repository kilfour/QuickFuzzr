using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.C_Cookbook.C_AHeapOfTrouble;

[DocFile]
[DocFileHeader("A Heap of Trouble")]
[DocContent(
@"A min-heap is a binary tree where every node's value is less than or equal to
the values of its children.

The children's valid value range depends on the value generated for their
parent. LINQ composition makes that dependency explicit.")]
[DocExample(typeof(HeapNode))]
[DocContent("The recursive generator receives the remaining `depth` and the current `minimum` value:")]
[DocExample(typeof(AHeapOfTrouble), nameof(HeapFuzzr))]
[DocContent(
@"`HeapFuzzr` first chooses the current value, then uses it as the lower bound
while recursively generating both children. The depth argument makes the
result a complete binary tree and gives the recursion an explicit stopping
point.")]
public class AHeapOfTrouble
{
    [CodeSnippet]
    [CodeRemove("private ")]
    private static FuzzrOf<HeapNode?> HeapFuzzr(int depth, int minimum)
    {
        if (depth == 0)
            return Fuzzr.Constant<HeapNode?>(null);

        return
            from value in Fuzzr.Int(minimum, 101)
            from left in HeapFuzzr(depth - 1, value)
            from right in HeapFuzzr(depth - 1, value)
            select new HeapNode(value, left, right);
    }

    [Fact]
    [DocContent("Generate heaps of varying depths:")]
    [DocExample(typeof(AHeapOfTrouble), nameof(GenerateHeap))]
    public void GeneratedTreesAreValidCompleteMinHeaps()
    {
        var heaps = GenerateHeap().Many(100).Generate(42);

        Assert.All(heaps, heap =>
        {
            Assert.True(IsMinHeap(heap));
            Assert.Equal((1 << Depth(heap)) - 1, Count(heap));
        });
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<HeapNode> GenerateHeap()
    {
        return
            from depth in Fuzzr.Int(2, 5)
            from heap in HeapFuzzr(depth, 1)
            select heap!;
    }

    private static bool IsMinHeap(HeapNode? node)
    {
        if (node is null) return true;

        return (node.Left is null || node.Value <= node.Left.Value) &&
               (node.Right is null || node.Value <= node.Right.Value) &&
               IsMinHeap(node.Left) &&
               IsMinHeap(node.Right);
    }

    private static int Count(HeapNode? node) =>
        node is null ? 0 : 1 + Count(node.Left) + Count(node.Right);

    private static int Depth(HeapNode? node) =>
        node is null ? 0 : 1 + Math.Max(Depth(node.Left), Depth(node.Right));
}
