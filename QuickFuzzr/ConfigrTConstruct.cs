using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static FuzzrOf<Intent> Construct(Func<T> ctor)
    {
        return state =>
        {
            var targetType = typeof(T);
            if (!state.Constructors.TryGetValue(targetType, out var list))
            {
                list = new List<Func<State, object>>();
                state.Constructors[targetType] = list;
            }

            list.Add(s => ctor()!);
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    public static FuzzrOf<Intent> Construct<TArg>(FuzzrOf<TArg> generator)
    {
        return state =>
        {
            var targetType = typeof(T);
            var ctor = targetType.GetConstructor([typeof(TArg)]);
            if (ctor == null)
                throw new InvalidOperationException($"No constructor found on {targetType} with argument of type {typeof(TArg)}");

            Func<State, object> ctorFunc = s =>
            {
                var arg = generator(s).Value!;
                return ctor.Invoke([arg]);
            };

            if (!state.Constructors.TryGetValue(targetType, out var list))
            {
                list = new List<Func<State, object>>();
                state.Constructors[targetType] = list;
            }

            list.Add(ctorFunc);
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    public static FuzzrOf<Intent> Construct<T1, T2>(FuzzrOf<T1> g1, FuzzrOf<T2> g2)
    {
        return state =>
        {
            var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2)]);
            if (ctor == null)
                throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)})");

            Func<State, object> fn = s =>
            {
                var arg1 = g1(s).Value!;
                var arg2 = g2(s).Value!;
                return ctor.Invoke([arg1, arg2]);
            };

            if (!state.Constructors.TryGetValue(typeof(T), out var list))
                state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

            list.Add(fn);
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    public static FuzzrOf<Intent> Construct<T1, T2, T3>(FuzzrOf<T1> g1, FuzzrOf<T2> g2, FuzzrOf<T3> g3)
    {
        return state =>
        {
            var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2), typeof(T3)]);
            if (ctor == null)
                throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)}, {typeof(T3)})");

            Func<State, object> fn = s =>
            {
                var arg1 = g1(s).Value!;
                var arg2 = g2(s).Value!;
                var arg3 = g3(s).Value!;
                return ctor.Invoke([arg1, arg2, arg3]);
            };

            if (!state.Constructors.TryGetValue(typeof(T), out var list))
                state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

            list.Add(fn);
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    public static FuzzrOf<Intent> Construct<T1, T2, T3, T4>(FuzzrOf<T1> g1, FuzzrOf<T2> g2, FuzzrOf<T3> g3, FuzzrOf<T4> g4)
    {
        return state =>
        {
            var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2), typeof(T3), typeof(T4)]);
            if (ctor == null)
                throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)}, {typeof(T3)}, {typeof(T4)})");

            Func<State, object> fn = s =>
            {
                var arg1 = g1(s).Value!;
                var arg2 = g2(s).Value!;
                var arg3 = g3(s).Value!;
                var arg4 = g4(s).Value!;
                return ctor.Invoke([arg1, arg2, arg3, arg4]);
            };

            if (!state.Constructors.TryGetValue(typeof(T), out var list))
                state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

            list.Add(fn);
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    public static FuzzrOf<Intent> Construct<T1, T2, T3, T4, T5>(FuzzrOf<T1> g1, FuzzrOf<T2> g2, FuzzrOf<T3> g3, FuzzrOf<T4> g4, FuzzrOf<T5> g5)
    {
        return state =>
        {
            var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)]);
            if (ctor == null)
                throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)}, {typeof(T3)}, {typeof(T4)}, {typeof(T5)})");

            Func<State, object> fn = s =>
            {
                var arg1 = g1(s).Value!;
                var arg2 = g2(s).Value!;
                var arg3 = g3(s).Value!;
                var arg4 = g4(s).Value!;
                var arg5 = g5(s).Value!;
                return ctor.Invoke([arg1, arg2, arg3, arg4, arg5]);
            };

            if (!state.Constructors.TryGetValue(typeof(T), out var list))
                state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

            list.Add(fn);
            return new Result<Intent>(Intent.Fixed, state);
        };
    }
}