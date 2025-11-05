using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that randomly selects one value from the provided options.
	/// Use for constrained choice scenarios like names, categories, or predefined enumerations.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(params T[] values)
	{
		return OneOf((IEnumerable<T>)values);
	}

	/// <summary>
	/// Creates a generator that randomly selects one value from the provided collection.
	/// Use when your choice data comes from lists, arrays, or other enumerable sources.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(IEnumerable<T> values)
	{
		return
			s =>
			{
				var index = Int(0, values.Count())(s).Value;
				return new Result<T>(values.ElementAt(index), s);
			};
	}

	/// <summary>
	/// Creates a generator that selects from available items or returns default if the collection is empty.
	/// Use for safe generation from potentially empty data sources without throwing exceptions.
	/// </summary>
	public static FuzzrOf<T> OneOfOrDefault<T>(IEnumerable<T> items)
	{
		var list = items;
		if (!list.Any())
			return Constant(default(T)!);
		return OneOf(list);
	}

	/// <summary>
	/// Creates a generator that randomly selects and executes one of the provided generators.
	/// Use for conditional generation strategies or when different value types require different generation logic.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(params FuzzrOf<T>[] values)
	{
		return
			s =>
			{
				var index = Int(0, values.Count())(s).Value;
				return new Result<T>(values.ElementAt(index)(s).Value, s);
			};
	}
}