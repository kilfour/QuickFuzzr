using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("Many")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Produces a number of values from the wrapped Fuzzr.")]
[DocContent("Produces a fixed number of values from a Fuzzr.")]
[DocSignature("ExtFuzzr.Many(this FuzzrOf<T> fuzzr, int number)")]
public class ExtFuzzrMany
{
    private sealed class RecursiveListNode
    {
        public List<RecursiveListNode> Children { get; set; } = [];
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<IEnumerable<int>> Fixed_Count_Fuzzr()
    {
        return Fuzzr.Constant(6).Many(3);
        // Results in => [6, 6, 6]
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ExtFuzzrMany), nameof(Fixed_Count_Fuzzr))]
    public void Fixed_Count()
    {
        var values = Fixed_Count_Fuzzr().Generate().ToArray();
        Assert.Equal(3, values.Length);
        Assert.True(values.All(v => v == 6));
    }

    [DocOverloads]
    [DocOverload("Many(this FuzzrOf<T> fuzzr, int min, int max)")]
    [DocContent("  Produces a variable number of values within bounds.")]
    [Fact]
    public void Range_Count()
    {
        var values = Fuzzr.Int().Many(1, 3).Generate(42).ToArray();
        Assert.InRange(values.Length, 1, 3);

        CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.Int().Many(1, 3),
            ("Count == 1", numbers => numbers.Count() == 1),
            ("Count == 2", numbers => numbers.Count() == 2),
            ("Count == 3", numbers => numbers.Count() == 3));
    }

    private static FuzzrOf<RecursiveListNode> RecursiveNodes(int minDepth, int maxDepth) =>
        from children in Configr<RecursiveListNode>.Property(
            node => node.Children,
            Fuzzr.One<RecursiveListNode>().Many(2).ToList())
        from depth in Configr<RecursiveListNode>.Depth(minDepth, maxDepth)
        from node in Fuzzr.One<RecursiveListNode>()
        select node;

    [Fact]
    [DocContent("- For recursive collections, `Many` returns an empty collection at the configured maximum depth.")]
    public void RecursiveCollectionStopsAtMaximumDepth()
    {
        var root = RecursiveNodes(1, 3).Generate();

        Assert.Equal(2, root.Children.Count);
        Assert.All(root.Children, child => Assert.Equal(2, child.Children.Count));
        Assert.All(root.Children.SelectMany(child => child.Children),
            grandchild => Assert.Empty(grandchild.Children));
    }

    [Fact]
    public void RecursiveCollectionAtMaximumDepthIsEmpty()
    {
        var root = RecursiveNodes(1, 1).Generate();

        Assert.Empty(root.Children);
    }

    [Fact]
    public void TopLevelManyIsNotLimitedByElementDepth()
    {
        var nodes =
            (from depth in Configr<RecursiveListNode>.Depth(1, 1)
             from values in Fuzzr.One<RecursiveListNode>().Many(5)
             select values)
            .Generate()
            .ToList();

        Assert.Equal(5, nodes.Count);
        Assert.All(nodes, node => Assert.Empty(node.Children));
    }
}
