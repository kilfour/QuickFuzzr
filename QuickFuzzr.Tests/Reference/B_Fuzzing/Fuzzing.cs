using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing;

[DocFile]
public class Fuzzing
{
    public static class Columns
    {
        public const string Fuzzr = "Fuzzr";
        public const string Description = "Description";
    }
    [DocContent(
@"This section lists the core generators responsible for object creation and composition.  
They provide controlled randomization, sequencing, and structural assembly beyond primitive value generation.  
Use these methods to instantiate objects, select values, define constants, or maintain counters during generation.  
All entries return a FuzzrOf<T> and can be composed using standard LINQ syntax.")]
    [DocHeader("Contents")]
    [DocTable(nameof(Methods), Columns.Fuzzr, Columns.Description)]
    private static void CheatSheet() {/* Placeholder */}
}