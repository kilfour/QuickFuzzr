using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a fuzzr that executes a side-effect action on each generated value without modifying the value itself.
    /// Use for performing operations like logging, adding to collections, or calling methods that have side effects but don't transform the data.
    /// </summary>
    public static FuzzrOf<Intent> Apply(Action<T> action)
        => state => Chain.It(() => state.StuffToApply[typeof(T)] = a => action((T)a), Result.Unit(state));
}