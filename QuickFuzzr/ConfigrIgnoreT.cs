using System.Linq.Expressions;
using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
	/// <summary>
	/// Creates a fuzzr that configures a specific property to be ignored during automatic generation.
	/// Use for excluding individual properties that should not be auto-populated, such as calculated fields or manually managed properties.
	/// </summary>
	public static FuzzrOf<Intent> Ignore<TProperty>(Expression<Func<T, TProperty>> func)
		=> state => Chain.It(() => state.StuffToIgnore.Add(func.AsPropertyInfo()), Result.Unit(state));
}