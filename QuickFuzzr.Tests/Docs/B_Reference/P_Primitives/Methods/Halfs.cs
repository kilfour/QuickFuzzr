using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Half()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random 16-bit floating-point numbers (default 1-100).")]
public class Halfs
{
    [Fact]
    [DocContent(
@"
- The overload Fuzzr.Half(Half min, Half max) generates a half-precision floating-point number greater than or equal to `min` and less than `max`.
  *Note:* Due to floating-point rounding, max may occasionally be produced.")]
    public void MinMax()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Half((Half)1, (Half)5),
            ("value >= 1", a => a >= (Half)1), ("value <= 5", a => a <= (Half)5));

    [Fact]
    [DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
    public void Throws()
        => Assert.Throws<ArgumentException>(() => Fuzzr.Half((Half)1, (Half)0).Generate());

    [Fact]
    [DocContent("- The default fuzzr is (min = (Half)1, max = (Half)100).")]
    public void DefaultFuzzr()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Half(),
            ("value >= 1", a => a >= (Half)1), ("value <= 100", a => a <= (Half)100));

    [Fact]
    public void Boundaries()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Half((Half)0, (Half)0.1),
            ("a >= 0", a => a >= (Half)0), ("a <= 0.1", a => a <= (Half)0.1));

    [Fact]
    public void Boundaries_Single()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Half((Half)0, (Half)0),
            ("a == 0", a => a == (Half)0));

    [Fact]
    [DocContent("- Can be made to return `Half?` using the `.Nullable()` combinator.")]
    public void Nullable()
        => CheckIf.GeneratesNullAndNotNull(Fuzzr.Half().Nullable());

    [Fact]
    [DocContent("- `Half` is automatically detected and generated for object properties.")]
    public void Property()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<Half>>(),
            ("value != 0", a => a.Value != (Half)0));

    [Fact]
    [DocContent("- `Half?` is automatically detected and generated for object properties.")]
    public void NullableProperty()
        => CheckIf.GeneratesNullAndNotNull(
            Fuzzr.One<PrimitivesBag<Half>>().Select(a => a.NullableValue));
}

