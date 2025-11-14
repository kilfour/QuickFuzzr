using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces unique values from the source fuzzr, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct identifiers, unique names, or any scenario requiring non-repeating values across multiple generations.
	/// Uses the globally configured retry limit.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> fuzzr, Func<object> key)
		=> Unique(fuzzr, key, null);

	/// <summary>
	/// Creates a fuzzr that produces unique values from the source fuzzr, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct identifiers, unique names, or any scenario requiring non-repeating values across multiple generations.
	/// Overrides the global retry limit for this fuzzr instance.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> fuzzr, Func<object> key, int maxAttempts)
		=> Unique(fuzzr, key, maxAttempts);

	/// <summary>
	/// Creates a fuzzr that produces unique values from the source fuzzr, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct values within specific contexts when you have a predetermined key for the uniqueness scope.
	/// Uses the globally configured retry limit.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> fuzzr, object key)
		=> Unique(fuzzr, () => key, null);

	/// <summary>
	/// Creates a fuzzr that produces unique values from the source fuzzr, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct values within specific contexts when you have a predetermined key for the uniqueness scope.
	/// Overrides the global retry limit for this fuzzr instance.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> fuzzr, object key, int maxAttempts)
		=> Unique(fuzzr, () => key, maxAttempts);

	private static FuzzrOf<T> Unique<T>(this FuzzrOf<T> fuzzr, Func<object> key, int? attempts = null)
	{
		return
			state =>
				{
					var limit = attempts ?? state.RetryLimit;
					var allreadyGenerated = state.Get(key(), new HashSet<T>());
					for (int i = 0; i < limit; i++)
					{
						var result = fuzzr(state);
						if (!allreadyGenerated.Contains(result.Value))
						{
							allreadyGenerated.Add(result.Value);
							return result;
						}
					}
					throw new UniqueValueExhaustedException(typeof(T).Name, key().ToString()!, limit);
				};
	}
}