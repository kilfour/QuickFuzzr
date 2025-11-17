using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged;

public abstract class RangedPrimitive<T> : Primitive<T>
    where T : struct, IComparable<T>
{
    protected abstract FuzzrOf<T> CreateRangedFuzzr(T min, T max);
    protected abstract (T Min, T Max) DefaultRange { get; }
    protected abstract (T Min, T Max) ExampleRange { get; }
    protected abstract (T Min, T Max) MinimalRange { get; }
    protected virtual bool UpperBoundExclusive => true;
    protected virtual bool SupportsEqualMinMax => true;
    protected virtual bool ThrowsOnMinGreaterThanMax => true;
    protected virtual T GetUpperBoundarySample(T min, T max) => max;
    protected virtual bool CheckExactBoundaries => true;

    [Fact]
    public virtual void DefaultFuzzr()
    {
        var (min, max) = DefaultRange;
        CheckIf.GeneratedValuesShouldAllSatisfy(CreateFuzzr(),
            ("value >= default min", a => a.CompareTo(min) >= 0),
            UpperBoundExclusive
                ? ("value < default max", a => a.CompareTo(max) < 0)
                : ("value <= default max", a => a.CompareTo(max) <= 0));
    }

    [Fact]
    public void MinMax()
    {
        var (min, max) = ExampleRange;
        CheckIf.GeneratedValuesShouldAllSatisfy(CreateRangedFuzzr(min, max),
            ("value >= min", a => a.CompareTo(min) >= 0),
            UpperBoundExclusive
                ? ("value < max", a => a.CompareTo(max) < 0)
                : ("value <= max", a => a.CompareTo(max) <= 0));
    }

    [Fact]
    public void MinMaxSame()
    {
        if (!SupportsEqualMinMax)
            return;
        var (min, _) = ExampleRange;
        CheckIf.GeneratedValuesShouldAllSatisfy(CreateRangedFuzzr(min, min),
            ("value == min", a => a.CompareTo(min) == 0));
    }

    [Fact]
    public void Boundaries()
    {
        if (!CheckExactBoundaries)
            return;
        var (min, max) = MinimalRange;
        var upper = GetUpperBoundarySample(min, max);
        CheckIf.GeneratedValuesShouldEventuallySatisfyAll(CreateRangedFuzzr(min, max),
            ("value == min", a => a.CompareTo(min) == 0),
            ("value == upper", a => a.CompareTo(upper) == 0));
    }

    [Fact]
    public void MinGreaterThanMax_Throws()
    {
        if (!ThrowsOnMinGreaterThanMax)
            return;
        var (min, max) = ExampleRange;
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateRangedFuzzr(max, min));
    }

    [Fact]
    public override void Property()
    {
        var (min, max) = ExampleRange;
        CheckIf.GeneratedValuesShouldAllSatisfy(
                from replace in Configr.Primitive(CreateRangedFuzzr(min, max))
                from one in Fuzzr.One<PrimitivesBag<T>>()
                select one,
            ("value >= example min", a => a.Value.CompareTo(min) >= 0),
            UpperBoundExclusive
                ? ("value < example max", a => a.Value.CompareTo(max) < 0)
                : ("value <= example max", a => a.Value.CompareTo(max) <= 0));
    }

    protected RangeBuilder From(T min) => new(min);
    protected class RangeBuilder(T min)
    {
        private readonly T min = min;
        public (T Min, T Max) To(T max) => (min, max);
    }
}