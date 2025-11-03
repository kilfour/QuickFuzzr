using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Byte()`.")]
public class Bytes
{
    [Fact]
    [DocContent("- The default generator produces a `byte` in the full range (`0`-`255`).")]
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
    [DocContent("- When `min == max`, the generator always returns that exact value.")]
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
    [DocContent("- Can be made to return `byte?` using the `.Nullable()` combinator.")]
    public void Nullable()
        => CheckIf.GeneratesNullAndNotNull(Fuzzr.Byte().Nullable());

    [Fact]
    [DocContent("- `byte` is automatically detected and generated for object properties.")]
    public void Property()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<ByteBag>(),
            ("Value in range", a => a.Value >= byte.MinValue && a.Value <= byte.MaxValue));

    [Fact]
    [DocContent("- `byte?` is automatically detected and generated for object properties.")]
    public void NullableProperty()
        => CheckIf.GeneratesNullAndNotNull(
            Fuzzr.One<ByteBag>().Select(a => a.NullableValue));

    // Local model mirroring the style used in the string tests.
    public class ByteBag
    {
        public byte Value { get; set; }
        public byte? NullableValue { get; set; }
    }
}
