using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random instances of type T using default construction rules.
	/// Use for zero-config generation of domain objects, value objects, and complex types.
	/// </summary>
	public static FuzzrOf<T> One<T>() =>
		state => new Result<T>((T)state.CreationEngine.Create(state, typeof(T)), state);

	/// <summary>
	/// Creates a Fuzzr that produces instances of type T using a custom constructor function.
	/// Use when you need explicit control over object creation or when working with types that lack parameterless constructors.
	/// </summary>
	public static FuzzrOf<T> One<T>(Func<T> factory)
	{
		ArgumentNullException.ThrowIfNull(factory, nameof(factory));
		return state => new Result<T>((T)state.CreationEngine.Create(state, typeof(T), _ => factory()!), state);
	}
}