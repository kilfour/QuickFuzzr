using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<T> One<T>()
		=> state =>
			new Result<T>((T)state.CreationEngine.Create(state, typeof(T)), state);

	// public static FuzzrOf<T> One<T>(FuzzrOf<Intent> config)
	// 	=> state =>
	// 		new Result<T>((T)state.CreationEngine.Create(state, typeof(T)), state);

	public static FuzzrOf<T> One<T>(Func<T> constructor)
		=> state =>
			new Result<T>((T)state.CreationEngine.Create(state, typeof(T), _ => constructor()!), state);
}