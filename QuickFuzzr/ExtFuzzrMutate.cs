using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class ExtFuzzrMutate
{
    /// <summary>
    /// Chooses one generator transformation for each generated value and repeats it.
    /// Supply (mutation, times) tuples for exact counts or (mutation, min, max) tuples
    /// for counts with an inclusive minimum and exclusive maximum; equal bounds give an exact count.
    /// Counts must be nonnegative. Zero repetitions preserve the source value.
    /// </summary>
    public static FuzzrOf<T> Mutate<T>(
        this FuzzrOf<T> fuzzr,
        params Mutation<T>[] mutations)
    {
        ArgumentNullException.ThrowIfNull(fuzzr);
        ArgumentNullException.ThrowIfNull(mutations);
        if (mutations.Length == 0)
            throw new ArgumentException("Supply at least one mutation.", nameof(mutations));

        return Fuzzr.OneOf(
                mutations
                    .Select(m => Repeat(fuzzr, m.Min, m.Max, m.Apply))
                    .ToArray());
    }

    /// <summary>Validates repetition bounds and repeats a transformation using a count drawn from that range.</summary>
    private static FuzzrOf<T> Repeat<T>(
        FuzzrOf<T> fuzzr,
        int min,
        int max,
        Func<FuzzrOf<T>, FuzzrOf<T>> mutation)
    {
        ArgumentNullException.ThrowIfNull(mutation);
        ArgumentOutOfRangeException.ThrowIfNegative(min);
        ArgumentOutOfRangeException.ThrowIfNegative(max);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);
        return fuzzr.Repeat(Fuzzr.Int(min, max), mutation);
    }

    /// <summary>Repeats a transformation using a fresh generated count for each generated value.</summary>
    private static FuzzrOf<T> Repeat<T>(
        this FuzzrOf<T> fuzzr,
        FuzzrOf<int> times,
        Func<FuzzrOf<T>, FuzzrOf<T>> mutation) =>
            from count in times
            from result in fuzzr.Repeat(count, mutation)
            select result;

    /// <summary>Composes a transformation the specified number of times, preserving the source when the count is zero.</summary>
    private static FuzzrOf<T> Repeat<T>(
        this FuzzrOf<T> fuzzr,
        int times,
        Func<FuzzrOf<T>, FuzzrOf<T>> mutation)
    {
        for (int i = 0; i < times; i++)
        {
            fuzzr = mutation(fuzzr);
        }
        return fuzzr;
    }
}
