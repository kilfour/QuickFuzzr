using System.Linq.Expressions;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Config<T>
{
    public static Generator<Unit> Ignore<TProperty>(Func<System.Reflection.PropertyInfo, bool> predicate)
        => s =>
            {
                //s.GeneralStuffToIgnore.Add(predicate);
                return new Result<Unit>(Unit.Instance, s);
            };
}