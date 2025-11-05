using System.Linq.Expressions;
using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
	/// <summary>
	/// Creates a generator that configures a specific property to be ignored during automatic generation.
	/// Use for excluding individual properties that should not be auto-populated, such as calculated fields or manually managed properties.
	/// </summary>
	public static FuzzrOf<Intent> Ignore<TProperty>(Expression<Func<T, TProperty>> func)
		=> state => Chain.It(() => state.StuffToIgnore.Add(func.AsPropertyInfo()), Result.Unit(state));

	/// <summary>
	/// Creates a generator that configures all properties of type T to be ignored during automatic generation.
	/// Use when you want to completely disable auto-generation for a specific type and handle all property population manually.
	/// </summary>
	public static FuzzrOf<Intent> IgnoreAll()
		=> state => Chain.It(() => state.StuffToIgnoreAll.Add(typeof(T)), Result.Unit(state));
}