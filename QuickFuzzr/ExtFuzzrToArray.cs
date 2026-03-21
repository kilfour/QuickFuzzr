namespace QuickFuzzr;

public static partial class ExtFuzzr
{
    /// <summary>
    /// Creates a Fuzzr that materializes the sequence produced by the source Fuzzr as an array.
    /// Use when you need fixed-size collection semantics, array APIs, or eager materialization from generated enumerable data.
    /// </summary>
    public static FuzzrOf<T[]> ToArray<T>(this FuzzrOf<IEnumerable<T>> fuzzr)
        => from elements in fuzzr select elements.ToArray();
}