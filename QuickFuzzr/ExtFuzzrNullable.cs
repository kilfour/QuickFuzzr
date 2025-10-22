using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> generator)
		where T : struct
	{
		return Nullable(generator, 0.2);
	}

	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> generator, double percentageOfNulls)
		where T : struct
	{
		return
			state =>
			{
				if (state.Random.NextDouble() < percentageOfNulls)
					return new Result<T?>(null, state);
				var val = generator(state).Value;
				return new Result<T?>(val, state);
			};
	}

	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> generator)
		where T : class
	{
		return NullableRef(generator, 0.2);
	}

	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> generator, double percentageOfNulls)
		where T : class
	{
		return
			state =>
			{
				if (state.Random.NextDouble() < percentageOfNulls)
					return new Result<T?>(null, state);
				var val = generator(state).Value;
				return new Result<T?>(val, state);
			};
	}

	public static FuzzrOf<T?> NeverReturnNull<T>(this FuzzrOf<T?> generator)
		where T : struct
	{
		return
			s =>
			{

				var val = generator(s).Value;
				var i = 0;
				while (val == null)
				{
					val = generator(s).Value;
					i++;
					if (i >= 50)
						throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue();
				}
				return new Result<T?>(val, s);
			};
	}
}