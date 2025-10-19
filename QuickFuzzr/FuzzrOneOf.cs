using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzzr
	{
		public static Generator<T> OneOf<T>(params T[] values)
		{
			return OneOf((IEnumerable<T>)values);
		}

		public static Generator<T> OneOf<T>(IEnumerable<T> values)
		{
			return
				s =>
				{
					var index = Int(0, values.Count())(s).Value;
					return new Result<T>(values.ElementAt(index), s);
				};
		}

		public static Generator<T> OneOfOrDefault<T>(IEnumerable<T> items)
		{
			var list = items;
			if (!list.Any())
				return Constant(default(T)!);
			return OneOf(list);
		}

		public static Generator<T> OneOf<T>(params Generator<T>[] values)
		{
			return
				s =>
				{
					var index = Int(0, values.Count())(s).Value;
					return new Result<T>(values.ElementAt(index)(s).Value, s);
				};
		}
	}
}