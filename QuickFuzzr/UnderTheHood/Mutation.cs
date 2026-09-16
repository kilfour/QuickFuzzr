namespace QuickFuzzr.UnderTheHood;

public readonly record struct Mutation<T>(
    Func<FuzzrOf<T>, FuzzrOf<T>> Apply,
    int Min = 1,
    int Max = 1)
{
    public static implicit operator Mutation<T>(
        (Func<FuzzrOf<T>, FuzzrOf<T>> Mutation, int Times) value) =>
        new(value.Mutation, value.Times, value.Times);

    public static implicit operator Mutation<T>(
        (Func<FuzzrOf<T>, FuzzrOf<T>> Mutation, int Min, int Max) value) =>
        new(value.Mutation, value.Min, value.Max);
}
