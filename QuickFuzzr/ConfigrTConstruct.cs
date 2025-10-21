using System.Collections.Concurrent;
using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    private static readonly ConcurrentDictionary<(Type Target, Type[] Args), ConstructorInfo> CtorCache = new();

    public static FuzzrOf<Intent> Construct(Func<T> ctor) => state => Add(state, _ => ctor()!);

    public static FuzzrOf<Intent> Construct<TArg>(FuzzrOf<TArg> fuzzr) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(TArg)], s => fuzzr(s).Value!));

    public static FuzzrOf<Intent> Construct<T1, T2>(FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!));

    public static FuzzrOf<Intent> Construct<T1, T2, T3>(
        FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2, FuzzrOf<T3> fuzzr3) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2), typeof(T3)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!, s => fuzzr3(s).Value!));

    public static FuzzrOf<Intent> Construct<T1, T2, T3, T4>(
        FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2, FuzzrOf<T3> fuzzr3, FuzzrOf<T4> fuzzr4) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2), typeof(T3), typeof(T4)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!, s => fuzzr3(s).Value!, s => fuzzr4(s).Value!));

    public static FuzzrOf<Intent> Construct<T1, T2, T3, T4, T5>(
        FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2, FuzzrOf<T3> fuzzr3, FuzzrOf<T4> fuzzr4, FuzzrOf<T5> fuzzr5) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!, s => fuzzr3(s).Value!, s => fuzzr4(s).Value!, s => fuzzr5(s).Value!));

    // -------------------------------------------------------------
    // helpers 
    // --
    private static Result<Intent> Add(State state, Func<State, object> ctorFunc)
    {
        var list = GetOrAddList(state, typeof(T));
        list.Add(ctorFunc);
        return new Result<Intent>(Intent.Fixed, state);
    }

    private static List<Func<State, object>> GetOrAddList(State state, Type targetType)
    {
        if (!state.Constructors.TryGetValue(targetType, out var list))
        {
            list = [];
            state.Constructors[targetType] = list;
        }
        return list;
    }

    private static Func<State, object> MakeCtorFunc(
        Type targetType, Type[] argTypes, params Func<State, object?>[] argFetchers)
    {
        var ci = GetCtor(targetType, argTypes);
        return s =>
        {
            var args = new object?[argFetchers.Length];
            for (int i = 0; i < argFetchers.Length; i++)
                args[i] = argFetchers[i](s);
            return ci.Invoke(args);
        };
    }

    private static ConstructorInfo GetCtor(Type targetType, Type[] argTypes)
    {
        return CtorCache.GetOrAdd((targetType, argTypes),
            key =>
            {
                var (t, args) = key;
                var ci = t.GetConstructor(args);
                if (ci is null)
                    throw new InvalidOperationException(
                        $"No constructor found on {t} with args ({string.Join(", ", args.Select(a => a.Name))}).");
                return ci;
            });
    }
}
