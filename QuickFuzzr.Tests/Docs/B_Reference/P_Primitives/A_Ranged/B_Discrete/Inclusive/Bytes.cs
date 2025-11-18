using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete.Inclusive;


[DocContent("Use `Fuzzr.Byte()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random bytes in the range 0-255 or within a custom range.")]
[DocContent("- **Default Range:** min = 1, max = 255.")]
public class Bytes : RangedPrimitive<byte>
{
    protected override FuzzrOf<byte> CreateFuzzr() => Fuzzr.Byte();
    protected override FuzzrOf<byte> CreateRangedFuzzr(byte min, byte max) => Fuzzr.Byte(min, max);
    protected override (byte Min, byte Max) DefaultRange => (byte.MinValue, byte.MaxValue);
    protected override (byte Min, byte Max) ExampleRange => (5, 7);
    protected override (byte Min, byte Max) MinimalRange => (0, 1);
    protected override bool UpperBoundExclusive => false;
}
