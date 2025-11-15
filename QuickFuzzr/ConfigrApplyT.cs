using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a Fuzzr that executes a side-effect action on each generated value without modifying the value itself.
    /// Use for performing operations like logging, adding to collections, or calling methods that have side effects but don't transform the data.
    /// </summary>
    public static FuzzrOf<Intent> Apply(Action<T> action)
    {
        var key = typeof(T);
        return state =>
        {
            state.StuffToApply[key] = o => action((T)o);
            return Result.Unit(state);
        };
    }
}