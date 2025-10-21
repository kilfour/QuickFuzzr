using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> generator)
		where T : struct
	{
		return Nullable(generator, 5);
	}

	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> generator, int timesBeforeResultIsNullAproximation)
		where T : struct
	{
		return
			s =>
			{
				if (s.Random.Next(0, timesBeforeResultIsNullAproximation) == 0)
					return new Result<T?>(null, s);
				var val = generator(s).Value;
				return new Result<T?>(val, s);
			};
	}

	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> generator)
		where T : class
	{
		return NullableRef(generator, 5);
	}

	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> generator, int timesBeforeResultIsNullAproximation)
		where T : class
	{
		return
			s =>
			{
				if (s.Random.Next(0, timesBeforeResultIsNullAproximation) == 0)
					return new Result<T?>(null, s);
				var val = generator(s).Value;
				return new Result<T?>(val, s);
			};
	}

	public static FuzzrOf<T?> NeverReturnNull<T>(this FuzzrOf<T?> generator)
		where T : struct
	{
		return
			s =>
			{

				var val = generator(s).Value;
				while (val == null)
					val = generator(s).Value;
				return new Result<T?>(val, s);
			};
	}
}