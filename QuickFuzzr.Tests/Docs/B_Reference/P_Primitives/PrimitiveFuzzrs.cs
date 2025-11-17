using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives;

[DocFile]
[DocContent(
@"QuickFuzzr includes built-in Fuzzrs for all common primitive types.
These cover the usual suspects: numbers, booleans, characters, strings, dates, times, ...  
All with sensible defaults and range-based overloads.
They form the foundation on which more complex Fuzzrs are composed, and are used automatically when generating object properties.

All range-based numeric fuzzrs follow .NET conventions: the lower bound is inclusive and the upper bound is exclusive, unless explicitly stated otherwise.

> *All primitive Fuzzrs automatically drive object property generation.
> Nullable and non-nullable variants are both supported.
> Each Fuzzr also supports `.Nullable(...)` / `.NullableRef(...)` as appropriate.*
")]
// [DocTable(nameof(Methods), Columns.Fuzzr, Columns.Description)]
// [DocTable(nameof(Ranged), Columns.Fuzzr, Columns.Description)]
public class PrimitiveFuzzrs
{
    public static class Columns
    {
        public const string Fuzzr = "Fuzzr";
        public const string Description = "Description";
    }
}
