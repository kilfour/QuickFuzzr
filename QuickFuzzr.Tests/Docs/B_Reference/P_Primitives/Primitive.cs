using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives;

public abstract class Primitive<T> where T : struct
{
    protected abstract FuzzrOf<T> CreateFuzzr();

    [Fact]
    public void Nullable()
        => CheckIf.GeneratesNullAndNotNull(CreateFuzzr().Nullable());

    [Fact]
    public virtual void Property()
        => CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<T>>(),
            ("value != default!", a => !EqualityComparer<T>.Default.Equals(a.Value, default!)));

    [Fact]
    public void NullableProperty()
        => CheckIf.GeneratesNullAndNotNull(
            Fuzzr.One<PrimitivesBag<T>>().Select(a => a.NullableValue));
}
