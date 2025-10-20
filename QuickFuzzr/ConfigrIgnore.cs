using System.Reflection;
using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    public static Generator<Unit> Ignore<TProperty>(Func<PropertyInfo, bool> predicate)
        => state => Chain.It(() => state.GeneralStuffToIgnore.Add(predicate), Result.Unit(state));
}