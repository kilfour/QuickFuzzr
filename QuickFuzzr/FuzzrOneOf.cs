using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<T> OneOf<T>(params T[] values)
	{
		return OneOf((IEnumerable<T>)values);
	}

	public static FuzzrOf<T> OneOf<T>(IEnumerable<T> values)
	{
		return
			s =>
			{
				var index = Int(0, values.Count())(s).Value;
				return new Result<T>(values.ElementAt(index), s);
			};
	}

	public static FuzzrOf<T> OneOfOrDefault<T>(IEnumerable<T> items)
	{
		var list = items;
		if (!list.Any())
			return Constant(default(T)!);
		return OneOf(list);
	}

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