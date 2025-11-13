using System.Collections.Concurrent;
using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a fuzzr that configures a single-parameter constructor for type T using the specified parameter fuzzr.
    /// Use for types that require constructor arguments when automatic parameterless construction is not available or desired.
    /// </summary>
    public static FuzzrOf<Intent> Construct<TArg>(FuzzrOf<TArg> fuzzr) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(TArg)], s => fuzzr(s).Value!));

    /// <summary>
    /// Creates a fuzzr that configures a two-parameter constructor for type T using the specified parameter fuzzrs.
    /// Use for types that require two constructor arguments, ensuring both parameters are generated according to their respective rules.
    /// </summary>
    public static FuzzrOf<Intent> Construct<T1, T2>(FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!));

    /// <summary>
    /// Creates a fuzzr that configures a three-parameter constructor for type T using the specified parameter fuzzrs.
    /// Use for complex types that require three constructor arguments with controlled generation for each parameter.
    /// </summary>
    public static FuzzrOf<Intent> Construct<T1, T2, T3>(
        FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2, FuzzrOf<T3> fuzzr3) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2), typeof(T3)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!, s => fuzzr3(s).Value!));

    /// <summary>
    /// Creates a fuzzr that configures a four-parameter constructor for type T using the specified parameter fuzzrs.
    /// Use for types with extensive constructor requirements where all four parameters need customized generation logic.
    /// </summary>
    public static FuzzrOf<Intent> Construct<T1, T2, T3, T4>(
        FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2, FuzzrOf<T3> fuzzr3, FuzzrOf<T4> fuzzr4) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2), typeof(T3), typeof(T4)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!, s => fuzzr3(s).Value!, s => fuzzr4(s).Value!));

    /// <summary>
    /// Creates a fuzzr that configures a five-parameter constructor for type T using the specified parameter fuzzrs.
    /// Use for complex domain objects or value types that require five constructor arguments with precise generation control.
    /// </summary>
    public static FuzzrOf<Intent> Construct<T1, T2, T3, T4, T5>(
        FuzzrOf<T1> fuzzr1, FuzzrOf<T2> fuzzr2, FuzzrOf<T3> fuzzr3, FuzzrOf<T4> fuzzr4, FuzzrOf<T5> fuzzr5) =>
        state => Add(state, MakeCtorFunc(typeof(T), [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)],
            s => fuzzr1(s).Value!, s => fuzzr2(s).Value!, s => fuzzr3(s).Value!, s => fuzzr4(s).Value!, s => fuzzr5(s).Value!));

    // -------------------------------------------------------------
    // helpers 
    // --
    private static Result<Intent> Add(State state, Func<State, object> ctorFunc)
    {
        state.Constructors[typeof(T)] = ctorFunc;
        return new Result<Intent>(Intent.Fixed, state);
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
    private static readonly ConcurrentDictionary<(Type Target, Type[] Args), ConstructorInfo> CtorCache = new();

    private static ConstructorInfo GetCtor(Type targetType, Type[] argTypes)
    {
        return CtorCache.GetOrAdd((targetType, argTypes),
            key =>
            {
                var (t, args) = key;
                var ci = t.GetConstructor(args);
                if (ci is null)
                    throw new InvalidOperationException(
                        $"No constructor found on {t.Name} with args ({string.Join(", ", args.Select(a => a.Name))}).");
                return ci;
            });
    }
}
