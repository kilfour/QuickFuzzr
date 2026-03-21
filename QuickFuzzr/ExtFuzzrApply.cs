namespace QuickFuzzr;

public static partial class ExtFuzzr
{
    /// <summary>
    /// Creates a Fuzzr that executes a side-effect action on each generated value without modifying the value itself.
    /// Use for performing operations like logging, adding to collections, or calling methods that have side effects but don't transform the data.
    /// </summary>
    public static FuzzrOf<T> Apply<T>(this FuzzrOf<T> fuzzr, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return state =>
        {
            var result = fuzzr(state);
            action(result.Value);
            return result;
        };
    }
}
