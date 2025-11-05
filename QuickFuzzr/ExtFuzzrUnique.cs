using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a generator that produces unique values from the source generator, ensuring no duplicates within the same generation scope.
	/// Use for generating distinct identifiers, unique names, or any scenario requiring non-repeating values across multiple generations.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> generator, Func<object> key)
	{
		return
			s =>
				{
					var allreadyGenerated = s.Get(key(), new List<T>());
					for (int i = 0; i < 50; i++)
					{
						var result = generator(s);
						if (!allreadyGenerated.Contains(result.Value))
						{
							allreadyGenerated.Add(result.Value);
							return result;
						}
					}
					throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue($"(key: {key()})");
				};
	}

	/// <summary>
	/// Creates a generator that produces unique values from the source generator using a fixed key for uniqueness tracking.
	/// Use for generating distinct values within specific contexts when you have a predetermined key for the uniqueness scope.
	/// </summary>
	public static FuzzrOf<T> Unique<T>(this FuzzrOf<T> generator, object key)
	{
		return
			s =>
				{
					var allreadyGenerated = s.Get(key, new List<T>());
					for (int i = 0; i < 50; i++)
					{
						var result = generator(s);
						if (!allreadyGenerated.Contains(result.Value))
						{
							allreadyGenerated.Add(result.Value);
							return result;
						}
					}
					throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue($"(key: {key})");
				};
	}
}