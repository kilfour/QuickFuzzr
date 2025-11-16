using QuickFuzzr.Tests.Docs.C_Cookbook.A_ADeepDarkForest;
using QuickFuzzr.Tests.Docs.C_Cookbook.B_SometimesTheCheetahNeedsToRun;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.C_Cookbook;

[DocFile]
[DocFileHeader("Cooking Up a Fuzzr")]
[DocLink(typeof(ADeepDarkForest), "ADeepDarkForest")]
[DocLink(typeof(SometimesTheCheetahNeedsToRun), "SometimesTheCheetahNeedsToRun")]
public class Cookbook
{
    [DocHeader("Contents")]
    [DocContent(
@"
- [A Deep Dark Forest][ADeepDarkForest]
- [Sometimes the Cheetah Needs to Run][SometimesTheCheetahNeedsToRun]
")]
    public static void Contents() { }
}