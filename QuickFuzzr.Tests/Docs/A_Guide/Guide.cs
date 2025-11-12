using QuickFuzzr.Tests.Docs.A_Guide.A_YourFirstFuzzr;
using QuickFuzzr.Tests.Docs.A_Guide.B_OnComposition;
using QuickFuzzr.Tests.Docs.A_Guide.C_BeautifullyCarvedObjects;
using QuickFuzzr.Tests.Docs.A_Guide.D_ThroughTheLookingGlass;
using QuickFuzzr.Tests.Docs.A_Guide.Q_ShowCase;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.A_Guide;

[DocFile]
[DocLink(typeof(YourFirstFuzzr), "YourFirstFuzzr")]
[DocLink(typeof(OnComposition), "OnComposition")]
[DocLink(typeof(BeautifullyCarvedObjects), "BeautifullyCarvedObjects")]
[DocLink(typeof(ThroughTheLookingGlass), "ThroughTheLookingGlass")]
[DocLink(typeof(TheFinalShowcase), "TheFinalShowcase")]
public class Guide
{
    [DocHeader("Contents")]
    [DocContent(
@"
- [Your First Fuzzr][YourFirstFuzzr]
- [On Composition][OnComposition]
- [Beautifully Carved Objects][BeautifullyCarvedObjects]
- [Through the Looking Glass][ThroughTheLookingGlass]
- [TheFinalShowcase][TheFinalShowcase]
")]
    public static void Contents() { }
}