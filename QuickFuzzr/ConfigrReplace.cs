using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<Intent> Replace<T>(this FuzzrOf<T> generator)
		where T : struct
	{
		return state =>
		{
			state.PrimitiveGenerators[typeof(T)] = generator.AsObject();
			state.PrimitiveGenerators[typeof(T?)] = generator.Nullable().AsObject();
			return new Result<Intent>(Intent.Fixed, state);
		};
	}

	public static FuzzrOf<Intent> Replace<T>(this FuzzrOf<T?> generator)
		where T : struct
	{
		return state =>
		{
			state.PrimitiveGenerators[typeof(T?)] = generator.AsObject();
			return new Result<Intent>(Intent.Fixed, state);
		};
	}

	public static FuzzrOf<Intent> Replace(this FuzzrOf<string> generator)
	{
		return state =>
		{
			state.PrimitiveGenerators[typeof(string)] = generator.AsObject();
			return new Result<Intent>(Intent.Fixed, state);
		};
	}
}