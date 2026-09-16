using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class ExtFuzzrMutate
{
    public static FuzzrOf<T> Mutate<T>(
        this FuzzrOf<T> fuzzr,
        params Mutation<T>[] mutations) =>
            Fuzzr.OneOf(
                mutations
                    .Select(m => Repeat(fuzzr, m.Min, m.Max, m.Apply))
                    .ToArray());

    private static FuzzrOf<T> Repeat<T>(
        FuzzrOf<T> fuzzr,
        int min,
        int max,
        Func<FuzzrOf<T>, FuzzrOf<T>> mutation) =>
            fuzzr.Repeat(Fuzzr.Int(min, max), mutation);

    private static FuzzrOf<T> Repeat<T>(
        this FuzzrOf<T> fuzzr,
        FuzzrOf<int> times,
        Func<FuzzrOf<T>, FuzzrOf<T>> mutation) =>
            from count in times
            from result in fuzzr.Repeat(count, mutation)
            select result;

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