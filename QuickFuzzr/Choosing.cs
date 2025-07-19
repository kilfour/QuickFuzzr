using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<T> ChooseFromThese<T>(params T[] values)
		{
			return ChooseFrom(values);
		}

		public static Generator<T> ChooseFrom<T>(IEnumerable<T> values)
		{
			return
				s =>
				{
					var index = Int(0, values.Count())(s).Value;
					return new Result<T>(values.ElementAt(index), s);
				};
		}

		public static Generator<T> ChooseFromWithDefaultWhenEmpty<T>(IEnumerable<T> items)
		{
			var list = items.ToList();
			if (list.Count == 0)
				return Constant(default(T)!);
			return ChooseFrom(list);
		}

		public static Generator<T> ChooseGenerator<T>(params Generator<T>[] values)
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