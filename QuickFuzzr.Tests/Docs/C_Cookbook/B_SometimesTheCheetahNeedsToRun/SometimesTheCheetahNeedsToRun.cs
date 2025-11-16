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

For other types, where property customization is heavier, the gains are noticeably larger.")]
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

    [DocHeader("Property Configurations Example:")]
    [DocContent("Suppose we have a class that has many properties, and we want to fuzz them all.")]
    [DocExample(typeof(Pseudopolis))]
    [DocContent("We could use *auto-fuzzing*:")]
    [DocExample(typeof(SometimesTheCheetahNeedsToRun), nameof(PseudopolisAutoFuzzr))]
    [DocContent("We could define a `Fuzzr` (I'm using the defaults for benchmarking purposes):")]
    [DocExample(typeof(SometimesTheCheetahNeedsToRun), nameof(PseudopolisFuzzr))]
    [DocContent("We could use a preloaded `Configr`:")]
    [DocExample(typeof(SometimesTheCheetahNeedsToRun), nameof(PseudopolisConfigr))]
    [DocContent("**Benchmarks:**")]
    [DocCodeFile("benchmarks-pseudopolis.txt", "markdown")]
    private static void PseudopolisExample() { }

    [CodeSnippet]
    [CodeRemove("return ")]
    public static IEnumerable<Pseudopolis> PseudopolisAutoFuzzr()
    {
        return Fuzzr.One<Pseudopolis>().Many(10000).Generate();
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    public static IEnumerable<Pseudopolis> PseudopolisFuzzr()
    {
        var fuzzr =
            from _ in Configr.IgnoreAll()
            from name in Configr<Pseudopolis>.Property(a => a.Name, Fuzzr.String())
            from nat in Configr<Pseudopolis>.Property(a => a.NaturalNumber, Fuzzr.Int())
            from money in Configr<Pseudopolis>.Property(a => a.Money, Fuzzr.Decimal())
            from date in Configr<Pseudopolis>.Property(a => a.Date, Fuzzr.DateTime())
            from flag in Configr<Pseudopolis>.Property(a => a.Boolean, Fuzzr.Bool())
            from pseudopolis in Fuzzr.One<Pseudopolis>()
            select pseudopolis;
        return fuzzr.Many(10000).Generate();
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    public static IEnumerable<Pseudopolis> PseudopolisConfigr()
    {
        var config =
            from _ in Configr.IgnoreAll()
            from name in Configr<Pseudopolis>.Property(a => a.Name, Fuzzr.String())
            from nat in Configr<Pseudopolis>.Property(a => a.NaturalNumber, Fuzzr.Int())
            from money in Configr<Pseudopolis>.Property(a => a.Money, Fuzzr.Decimal())
            from date in Configr<Pseudopolis>.Property(a => a.Date, Fuzzr.DateTime())
            from flag in Configr<Pseudopolis>.Property(a => a.Boolean, Fuzzr.Bool())
            select Intent.Fixed;
        var fuzzr =
            from _ in config
            from pseudopolis in Fuzzr.One<Pseudopolis>().Many(10000)
            select pseudopolis;
        return fuzzr.Generate();
    }


    [DocHeader("Summary:")]
    [DocContent(@"
QuickFuzzr's dynamic configuration is usually fast enough, and you rarely need to optimize.  
But when you do: **lifting Configr calls out of the hot path** moves QuickFuzzr into the upper end of the performance spectrum.
")]
    private static void Summary() { }
}