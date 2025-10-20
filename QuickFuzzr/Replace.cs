using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<Unit> Replace<T>(this Generator<T> generator)
		where T : struct
	{
		return state =>
		{
			state.PrimitiveGenerators[typeof(T)] = generator.AsObject();
			state.PrimitiveGenerators[typeof(T?)] = generator.Nullable().AsObject();
			return new Result<Unit>(Unit.Instance, state);
		};
	}

	public static Generator<Unit> Replace<T>(this Generator<T?> generator)
		where T : struct
	{
		return state =>
		{
			state.PrimitiveGenerators[typeof(T?)] = generator.AsObject();
			return new Result<Unit>(Unit.Instance, state);
		};
	}

	public static Generator<Unit> Replace(this Generator<string> generator)
	{
		return state =>
		{
			state.PrimitiveGenerators[typeof(string)] = generator.AsObject();
			return new Result<Unit>(Unit.Instance, state);
		};
	}
}