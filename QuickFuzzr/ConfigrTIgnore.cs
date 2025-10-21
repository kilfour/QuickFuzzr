using System.Linq.Expressions;
using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
	public static FuzzrOf<Intent> Ignore<TProperty>(Expression<Func<T, TProperty>> func)
		=> state => Chain.It(() => state.StuffToIgnore.Add(func.AsPropertyInfo()), Result.Unit(state));

	public static FuzzrOf<Intent> IgnoreAll()
		=> state => Chain.It(() => state.StuffToIgnoreAll.Add(typeof(T)), Result.Unit(state));
}