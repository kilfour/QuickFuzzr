using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	private static FuzzrOf<T> PrimitiveDefault<T>(FuzzrOf<T> builtIn) =>
		state =>
		{
			if (!state.PrimitiveFuzzrs.TryGetValue(typeof(T), out var configured))
				return builtIn(state);

			return new Result<T>((T)configured(state).Value, state);
		};
}
