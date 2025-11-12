using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a generator that configures depth constraints for type T to control recursive object graph generation.
    /// Use for preventing infinite recursion in complex nested structures and ensuring generation terminates at reasonable depths.
    /// </summary>
    public static FuzzrOf<Intent> Depth(int min, int max)
    {
        if (min < 0)
            throw new ArgumentOutOfRangeException(nameof(min), $"Minimum depth must be non-negative for type {typeof(T).Name}.");
        if (min > max)
            throw new ArgumentOutOfRangeException(nameof(max), $"Maximum depth must be greater than or equal to minimum depth for type {typeof(T).Name}.");
        return state =>
            {
                state.DepthConstraints[typeof(T)] = new(min, max);
                return Result.Unit(state);
            };
    }
}