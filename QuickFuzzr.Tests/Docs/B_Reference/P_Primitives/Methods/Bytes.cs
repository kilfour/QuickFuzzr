using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Byte()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random bytes in the range 0-255 or within a custom range.")]
public class Bytes
{
    [Fact]
    [DocContent("- The default Fuzzr produces a `byte` in the full range (`0`-`255`).")]
    public void DefaultRange()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Byte(),
            (">= byte.MinValue", a => a >= byte.MinValue),
            ("<= byte.MaxValue", a => a <= byte.MaxValue));

    [Fact]
    [DocContent("- The overload `Fuzzr.Byte(int min, int max)` generates a value greater than or equal to `min` and less than or equal to `max`.")]
    public void MinMax()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Byte(5, 7),
            (">= 5", a => a >= 5),
            ("<= 7", a => a <= 7));

    [Fact]
    [DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
    public void MinGreaterThanMax_Throws()
        => Assert.Throws<ArgumentException>(() => Fuzzr.Byte(10, 9).Generate());

    [Fact]
    [DocContent("- Throws an `ArgumentOutOfRangeException` when `min` < `byte.MinValue` (i.e. `< 0`).")]
    public void MinBelowByteMin_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Fuzzr.Byte(-1, 10).Generate());

    [Fact]
    [DocContent("- Throws an `ArgumentOutOfRangeException` when `max` > `byte.MaxValue` (i.e. `> 255`).")]
    public void MaxAboveByteMax_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Fuzzr.Byte(0, 256).Generate());

    [Fact]
    [DocContent("- When `min == max`, the Fuzzr always returns that exact value.")]
    public void MinMaxEqual()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Byte(42, 42),
            ("== 42", a => a == 42));

    [Fact]
    [DocContent("- Boundary coverage: over time, values at both ends of the interval should appear.")]
    public void Boundaries()
        => CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.Byte(0, 1),
            ("== 0", a => a == 0),
            ("== 1", a => a == 1));

    [Fact]
    public void Nullable()
        => CheckIf.GeneratesNullAndNotNull(Fuzzr.Byte().Nullable());

    [Fact]
    public void Property()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<byte>>(),
            ("Value in range", a => a.Value >= byte.MinValue && a.Value <= byte.MaxValue));

    [Fact]
    public void NullableProperty()
        => CheckIf.GeneratesNullAndNotNull(
            Fuzzr.One<PrimitivesBag<byte>>().Select(a => a.NullableValue));

}
