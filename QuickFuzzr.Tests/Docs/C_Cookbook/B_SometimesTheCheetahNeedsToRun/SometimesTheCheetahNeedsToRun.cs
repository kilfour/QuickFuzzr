using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.Tests.Docs.C_Cookbook.A_ADeepDarkForest;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.C_Cookbook.B_SometimesTheCheetahNeedsToRun;

[DocFile]
[DocFileHeader("Sometimes the Cheetah Needs to Run")]
[DocContent(
@"Most of the time, QuickFuzzr is fast enough.  
Sure there's faster out there, and nothing beats a hand-rolled generator, but QuickFuzzr lands comfortably somewhere in the middle.  

And that's fine: it's built for **agility**, not raw speed.

But then again... *sometimes the cheetah needs to run*.
")]
[DocHeader("Example: The Forest of a Thousand Trees")]
[DocContent("We could reuse the tree fuzzr from *\"A Deep Dark Forest\"* and simply do:")]
[DocExample(typeof(SometimesTheCheetahNeedsToRun), nameof(ConfigrFuzzr))]
[DocContent(
@"But since the configuration doesn't change between runs, 
we can optimize the `LINQ` chain a bit by **preloading** the config:")]
[DocExample(typeof(SometimesTheCheetahNeedsToRun), nameof(TreesTest_Configr))]
[DocContent("Now we build the forest with the configuration already fixed in place:")]
[DocExample(typeof(SometimesTheCheetahNeedsToRun), nameof(PreloadedConfigrFuzzr))]
[DocContent("**Benchmarks:**")]
[DocCodeFile("Benchmarks.txt", "markdown")]
[DocContent(@"
Not bad, considering this example tree model has no properties at all.

For more complex types, where property customization and recursion are heavier, the gains are noticeably larger.")]
[DocHeader("Summary:")]
[DocContent(@"
QuickFuzzr's dynamic configuration is usually fast enough, and you rarely need to optimize.  
But when you do: **lifting Configr calls out of the hot path** moves QuickFuzzr into the upper end of the performance spectrum.
")]
public class SometimesTheCheetahNeedsToRun
{
    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("42")]
    public static IEnumerable<Tree> ConfigrFuzzr(FuzzrOf<Tree> treefuzzr)
    {
        return treefuzzr.Many(1000).Generate(42);
    }

    [CodeSnippet]
    [CodeRemove("return treeConfigr;")]
    public static FuzzrOf<Intent> TreesTest_Configr()
    {
        var treeConfigr =
            from depth in Configr<Tree>.Depth(2, 5)
            from inheritance in Configr<Tree>.AsOneOf(typeof(Branch), typeof(Leaf))
            from terminator in Configr<Tree>.EndOn<Leaf>()
            select Intent.Fixed;
        return treeConfigr;
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("42")]
    public static IEnumerable<Tree> PreloadedConfigrFuzzr(FuzzrOf<Intent> treeConfigr)
    {
        var forestFuzzr =
            from cfg in treeConfigr
            from trees in Fuzzr.One<Tree>().Many(1000)
            select trees;
        return forestFuzzr.Generate(42);
    }



    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<IEnumerable<Tree>> TreesTest_Fuzzr(FuzzrOf<Intent> treeConfigr)
    {
        return
            from _ in treeConfigr
            from trees in Fuzzr.One<Tree>().Many(100)
            select trees;
    }
}