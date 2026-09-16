namespace QuickFuzzr.UnderTheHood;

public readonly record struct Mutation<T>(
    Func<FuzzrOf<T>, FuzzrOf<T>> Apply,
    int Min = 1,
    int Max = 1)
{
    /// <summary>Converts a transformation and exact repetition count from tuple form.</summary>
    public static implicit operator Mutation<T>(
        (Func<FuzzrOf<T>, FuzzrOf<T>> Mutation, int Times) value) =>
        new(value.Mutation, value.Times, value.Times);

    /// <summary>Converts a transformation and repetition bounds from tuple form.</summary>
    public static implicit operator Mutation<T>(
        (Func<FuzzrOf<T>, FuzzrOf<T>> Mutation, int Min, int Max) value) =>
        new(value.Mutation, value.Min, value.Max);
}
