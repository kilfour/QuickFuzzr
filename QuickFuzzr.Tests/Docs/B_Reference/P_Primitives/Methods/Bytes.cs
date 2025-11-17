using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Byte()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random bytes in the range 0-255 or within a custom range.")]
[DocContent("- The default Fuzzr produces a `byte` in the full range (`0`-`255`).")]
[DocOverloads]
[DocOverload("Fuzzr.Byte(int min, int max)")]
[DocContent("  Generates a value greater than or equal to `min` and less than or equal to `max`.")]
[DocContent("  Boundary coverage: over time, values at both ends of the interval should appear.")]
[DocExceptions]
[DocException("ArgumentOutOfRangeException", "When `min` > `max`.")]
[DocException("ArgumentOutOfRangeException", "When `min` < `byte.MinValue` (i.e. `< 0`).")]
[DocException("ArgumentOutOfRangeException", "When `max` > `byte.MaxValue` (i.e. `> 255`).")]
public class Bytes : RangedPrimitive<byte>
{
    protected override FuzzrOf<byte> CreateFuzzr() => Fuzzr.Byte();
    protected override FuzzrOf<byte> CreateRangedFuzzr(byte min, byte max) => Fuzzr.Byte(min, max);
    protected override (byte Min, byte Max) DefaultRange => (byte.MinValue, byte.MaxValue);
    protected override (byte Min, byte Max) ExampleRange => (5, 7);
    protected override (byte Min, byte Max) MinimalRange => (0, 1);
    protected override bool UpperBoundExclusive => false;
}
