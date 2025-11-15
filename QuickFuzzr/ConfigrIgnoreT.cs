using System.Linq.Expressions;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
	/// <summary>
	/// Creates a Fuzzr that configures a specific property to be ignored during automatic generation.
	/// Use for excluding individual properties that should not be auto-populated, such as calculated fields or manually managed properties.
	/// </summary>
	public static FuzzrOf<Intent> Ignore<TProperty>(Expression<Func<T, TProperty>> expression)
	{
		ArgumentNullException.ThrowIfNull(expression);
		return state =>
		{
			state.StuffToIgnore.Add(expression.AsPropertyInfo());
			return Result.Unit(state);
		};
	}
}