using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{

    /// <summary>
	/// Registers a fuzzr as the default for a value type and its nullable counterpart.
	/// Use for customizing primitive value generation across all auto-generated properties of the specified type.
	/// </summary>
	public static FuzzrOf<Intent> Primitive<T>(this FuzzrOf<T> fuzzr)
        where T : struct
    {
        return state =>
        {
            state.PrimitiveGenerators[typeof(T)] = fuzzr.AsObject();
            state.PrimitiveGenerators[typeof(T?)] = fuzzr.Nullable().AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    /// <summary>
    /// Registers a fuzzr as the default for a nullable value type.
    /// Use for customizing nullable value type generation with specific null probability or value ranges.
    /// </summary>
    public static FuzzrOf<Intent> Primitive<T>(this FuzzrOf<T?> fuzzr)
        where T : struct
    {
        return state =>
        {
            state.PrimitiveGenerators[typeof(T?)] = fuzzr.AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    /// <summary>
    /// Registers a fuzzr as the default string fuzzr.
    /// Use for customizing string generation with specific patterns, lengths, or character sets across all auto-generated string properties.
    /// </summary>
    public static FuzzrOf<Intent> Primitive(this FuzzrOf<string> fuzzr)
    {
        return state =>
        {
            state.PrimitiveGenerators[typeof(string)] = fuzzr.AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }
}