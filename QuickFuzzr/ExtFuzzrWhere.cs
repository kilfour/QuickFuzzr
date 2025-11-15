using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a Fuzzr that filters values from the source Fuzzr, only producing values that satisfy the predicate.
	/// Use for constraining generated data to specific conditions, business rules, or validation criteria within your test scenarios.
	/// </summary>
	public static FuzzrOf<T> Where<T>(this FuzzrOf<T> fuzzr, Func<T, bool> predicate)
	{
		return s =>
		{
			var limit = s.RetryLimit;
			for (var i = 0; i < limit; i++)
			{
				var candidate = fuzzr(s);
				if (predicate(candidate.Value))
					return candidate;
			}
			throw new PredicateUnsatisfiedException(typeof(T).Name, limit);
		};
	}
}