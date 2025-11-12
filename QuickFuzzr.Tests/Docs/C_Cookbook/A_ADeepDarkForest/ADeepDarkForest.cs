using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;
using QuickPulse.Show;

namespace QuickFuzzr.Tests.Docs.C_Cookbook.A_ADeepDarkForest;

[DocFile]
public class ADeepDarkForest
{
    private readonly bool WriteToFile = false;

    [Fact]
    [DocContent(
@"Using `Fuzzr<T>.Depth()` together with the `Fuzzr<T>.AsOneOf(...)` combinator and `Fuzzr<T>.EndOn<TEnd>()`
allows you to build tree type hierarchies.  
Given the canonical abstract `Tree`, concrete `Branch` and `Leaf` example model: ")]
    [DocCodeFile("Forest.cs", "csharp", 2, "QuickFuzzr.Tests\\_Tools\\Models\\")]
    [DocContent("We can generate a forest like so:")]
    [DocExample(typeof(ADeepDarkForest), nameof(TreesTest_Fuzzr))]
    [DocOutput]
    [DocCode(
@"
Branch
  ├── Branch
  │   ├── Leaf
  │   └── Branch
  │       ├── Leaf
  │       └── Leaf
  └── Leaf", "text")]
    public void TreesTest()
    {
        var tree = TreesTest_Fuzzr().Generate(42);
        Assert.NotNull(tree);

        var branch = Assert.IsType<Branch>(tree);

        Assert.NotNull(branch.Left);
        var branchL = Assert.IsType<Branch>(branch.Left);

        Assert.NotNull(branchL.Left);
        Assert.IsType<Leaf>(branchL.Left);

        Assert.NotNull(branchL.Right);
        var branchLR = Assert.IsType<Branch>(branchL.Right);

        Assert.NotNull(branchLR.Left);
        Assert.IsType<Leaf>(branchLR.Left);
        Assert.NotNull(branchLR.Right);
        Assert.IsType<Leaf>(branchLR.Right);

        Assert.NotNull(branch.Right);
        Assert.IsType<Leaf>(branch.Right);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Tree> TreesTest_Fuzzr()
    {
        return
            from depth in Configr<Tree>.Depth(2, 5)
            from inheritance in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
            from terminator in Configr<Tree>.EndOn<Leaf>()
            from tree in Fuzzr.One<Tree>()
            select tree;
    }

    [Fact]
    public void Trees_PropertyTest()
    {
        var generator =
            from depth in Configr<Tree>.Depth(1, 3)
            from inheritance in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
            from terminator in Configr<Tree>.EndOn<Leaf>()
            from tree in Fuzzr.One<Tree>()
            select tree.ToLabel();
        var validLabels = new[] { "E", "LE", "RE", "LLE", "LRE", "RLE", "RRE" };
        CheckIf.GeneratedValuesShouldEventuallySatisfyAll(200,
            generator,
            ("has E", s => s.Split("|").Contains("E")),
            ("has LE", s => s.Split("|").Contains("LE")),
            ("has RE", s => s.Split("|").Contains("RE")),
            ("has LLE", s => s.Split("|").Contains("LLE")),
            ("has LRE", s => s.Split("|").Contains("LRE")),
            ("has RLE", s => s.Split("|").Contains("RLE")),
            ("has RRE", s => s.Split("|").Contains("RRE")),
            ("valid", s => s.Split("|").All(validLabels.Contains))
        );
        if (WriteToFile)
            new PrettyDeep(a => ((Tree)a).ToTreeLabel())
                .Sculpt((
                         from depth in Configr<Tree>.Depth(1, 5)
                         from inheritance in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
                         from terminator in Configr<Tree>.EndOn<Leaf>()
                         from tree in Fuzzr.One<Tree>()
                         select tree).Generate(2))
                .PulseToLog("tree.log");
    }
}