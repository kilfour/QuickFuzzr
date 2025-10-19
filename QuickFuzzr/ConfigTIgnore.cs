using System.Linq.Expressions;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Config<T>
{
    public static Generator<Unit> Ignore<TProperty>(Expression<Func<T, TProperty>> func)
        => s =>
            {
                s.StuffToIgnore.Add(func.AsPropertyInfo());
                return new Result<Unit>(Unit.Instance, s);
            };
}