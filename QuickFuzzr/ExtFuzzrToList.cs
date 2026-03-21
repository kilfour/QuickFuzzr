namespace QuickFuzzr;

public static partial class ExtFuzzr
{
    /// <summary>
    /// Creates a Fuzzr that materializes the sequence produced by the source Fuzzr as a <see cref="List{T}"/>.
    /// Use when you need list-specific operations, index-based access, or eager collection materialization from generated enumerable data.
    /// </summary>
    public static FuzzrOf<List<T>> ToList<T>(this FuzzrOf<IEnumerable<T>> fuzzr)
        => from elements in fuzzr select elements.ToList();
}