using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.A_Primitives;

[DocFile]
[DocContent(
@"QuickFuzzr includes built-in fuzzrs for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex fuzzrs are composed, and are used automatically when generating object properties.
")]
[DocTable(nameof(Methods), Columns.Fuzzr, Columns.Description)]
public class PrimitiveFuzzrs
{
    public static class Columns
    {
        public const string Fuzzr = "Fuzzr";
        public const string Description = "Description";
    }
}