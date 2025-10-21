using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static FuzzrOf<T> Apply<T>(this FuzzrOf<T> generator, Action<T> action)
	{
		return
			s =>
				{
					var result = generator(s);
					action(result.Value);
					return result;
				};
	}

	public static FuzzrOf<T> Apply<T>(this FuzzrOf<T> generator, Func<T, T> func)
	{
		return
			s =>
			{
				var result = generator(s);
				return new Result<T>(func(result.Value), s);
			};
	}
}