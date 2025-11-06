using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{

    /// <summary>
	/// Registers a generator as the default for a value type and its nullable counterpart.
	/// Use for customizing primitive value generation across all auto-generated properties of the specified type.
	/// </summary>
	public static FuzzrOf<Intent> Primitive<T>(this FuzzrOf<T> generator)
        where T : struct
    {
        return state =>
        {
            state.PrimitiveGenerators[typeof(T)] = generator.AsObject();
            state.PrimitiveGenerators[typeof(T?)] = generator.Nullable().AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    /// <summary>
    /// Registers a generator as the default for a nullable value type.
    /// Use for customizing nullable value type generation with specific null probability or value ranges.
    /// </summary>
    public static FuzzrOf<Intent> Primitive<T>(this FuzzrOf<T?> generator)
        where T : struct
    {
        return state =>
        {
            state.PrimitiveGenerators[typeof(T?)] = generator.AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    /// <summary>
    /// Registers a generator as the default string generator.
    /// Use for customizing string generation with specific patterns, lengths, or character sets across all auto-generated string properties.
    /// </summary>
    public static FuzzrOf<Intent> Primitive(this FuzzrOf<string> generator)
    {
        return state =>
        {
            state.PrimitiveGenerators[typeof(string)] = generator.AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }
}