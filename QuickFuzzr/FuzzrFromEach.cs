using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
    public static FuzzrOf<IEnumerable<U>> FromEach<T, U>(IEnumerable<T> source, Func<T, FuzzrOf<U>> func) =>
        state =>
        {
            var values = new List<U>();
            foreach (var element in source)
            {
                values.Add(func(element)(state).Value);
            }
            return new Result<IEnumerable<U>>(values, state);
        };
}