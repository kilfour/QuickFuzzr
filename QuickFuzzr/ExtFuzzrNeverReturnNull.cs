using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a fuzzr that filters out null values from a nullable fuzzr, ensuring only non-null values are produced.
	/// Use when you need guaranteed non-null values for testing code paths that require valid data or cannot handle null inputs.
	/// </summary>
	public static FuzzrOf<T?> NeverReturnNull<T>(this FuzzrOf<T?> fuzzr) where T : struct
	{
		return s =>
		{
			var limit = s.RetryLimit;
			for (var i = 0; i < limit; i++)
			{
				var v = fuzzr(s).Value;
				if (v is not null)
					return new Result<T?>(v, s);
			}
			throw new NonNullValueExhaustedException(typeof(T).Name, limit);
		};
	}

	// public static FuzzrOf<T?> NeverReturnNullRef<T>(this FuzzrOf<T?> fuzzr) where T : class
	// {
	// 	return s =>
	// 	{
	// 		var limit = s.RetryLimit;
	// 		for (var i = 0; i < limit; i++)
	// 		{
	// 			var v = fuzzr(s).Value;
	// 			if (v is not null)
	// 				return new Result<T?>(v, s);
	// 		}
	// 		throw new NonNullValueExhaustedException(typeof(T).Name, limit);
	// 	};
	// }
}