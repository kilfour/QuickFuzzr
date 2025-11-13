using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a fuzzr that executes a side-effect action on each generated value without modifying the value itself.
	/// Use for performing operations like logging, adding to collections, or calling methods that have side effects but don't transform the data.
	/// </summary>
	public static FuzzrOf<T> Apply<T>(this FuzzrOf<T> fuzzr, Action<T> action)
	{
		return
			s =>
				{
					var result = fuzzr(s);
					action(result.Value);
					return result;
				};
	}

	/// <summary>
	/// Creates a fuzzr that transforms each generated value using the provided function.
	/// Use for modifying or enriching generated data while maintaining the generation context and state.
	/// </summary>
	public static FuzzrOf<T> Apply<T>(this FuzzrOf<T> fuzzr, Func<T, T> func)
	{
		return
			s =>
			{
				var result = fuzzr(s);
				return new Result<T>(func(result.Value), s);
			};
	}
}
