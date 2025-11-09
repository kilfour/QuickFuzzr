using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a generator that produces unique values from the source generator, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct identifiers, unique names, or any scenario requiring non-repeating values across multiple generations.
	/// Uses the globally configured retry limit.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> generator, Func<object> key)
		=> Unique(generator, key, null);

	/// <summary>
	/// Creates a generator that produces unique values from the source generator, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct identifiers, unique names, or any scenario requiring non-repeating values across multiple generations.
	/// Overrides the global retry limit for this generator instance.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> generator, Func<object> key, int maxAttempts)
		=> Unique(generator, key, maxAttempts);

	/// <summary>
	/// Creates a generator that produces unique values from the source generator, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct values within specific contexts when you have a predetermined key for the uniqueness scope.
	/// Uses the globally configured retry limit.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> generator, object key)
		=> Unique(generator, () => key, null);

	/// <summary>
	/// Creates a generator that produces unique values from the source generator, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct values within specific contexts when you have a predetermined key for the uniqueness scope.
	/// Overrides the global retry limit for this generator instance.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> generator, object key, int maxAttempts)
		=> Unique(generator, () => key, maxAttempts);

	private static FuzzrOf<T> Unique<T>(this FuzzrOf<T> generator, Func<object> key, int? attempts = null)
	{
		return
			state =>
				{
					var limit = attempts ?? state.RetryLimit;
					var allreadyGenerated = state.Get(key(), new List<T>());
					for (int i = 0; i < limit; i++)
					{
						var result = generator(state);
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