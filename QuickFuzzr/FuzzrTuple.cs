namespace QuickFuzzr;

public static partial class Fuzzr
{
    /// <summary>
    /// Creates a Fuzzr that combines two source Fuzzrs into a tuple of their generated values.
    /// Use for fixed-size heterogeneous input generation where two independently generated values should be produced together.
    /// </summary>
    public static FuzzrOf<(T1, T2)> Tuple<T1, T2>(
        FuzzrOf<T1> fuzzrOfT1,
        FuzzrOf<T2> fuzzrOfT2) =>
            from t1 in fuzzrOfT1
            from t2 in fuzzrOfT2
            select (t1, t2);

    /// <summary>
    /// Creates a Fuzzr that combines three source Fuzzrs into a tuple of their generated values.
    /// Use for fixed-size heterogeneous input generation where three independently generated values should be produced together.
    /// </summary>
    public static FuzzrOf<(T1, T2, T3)> Tuple<T1, T2, T3>(
        FuzzrOf<T1> fuzzrOfT1,
        FuzzrOf<T2> fuzzrOfT2,
        FuzzrOf<T3> fuzzrOfT3) =>
            from t1 in fuzzrOfT1
            from t2 in fuzzrOfT2
            from t3 in fuzzrOfT3
            select (t1, t2, t3);

    /// <summary>
    /// Creates a Fuzzr that combines four source Fuzzrs into a tuple of their generated values.
    /// Use for fixed-size heterogeneous input generation where four independently generated values should be produced together.
    /// </summary>
    public static FuzzrOf<(T1, T2, T3, T4)> Tuple<T1, T2, T3, T4>(
        FuzzrOf<T1> fuzzrOfT1,
        FuzzrOf<T2> fuzzrOfT2,
        FuzzrOf<T3> fuzzrOfT3,
        FuzzrOf<T4> fuzzrOfT4) =>
            from t1 in fuzzrOfT1
            from t2 in fuzzrOfT2
            from t3 in fuzzrOfT3
            from t4 in fuzzrOfT4
            select (t1, t2, t3, t4);

    /// <summary>
    /// Creates a Fuzzr that combines five source Fuzzrs into a tuple of their generated values.
    /// Use for fixed-size heterogeneous input generation where five independently generated values should be produced together.
    /// </summary>
    public static FuzzrOf<(T1, T2, T3, T4, T5)> Tuple<T1, T2, T3, T4, T5>(
        FuzzrOf<T1> fuzzrOfT1,
        FuzzrOf<T2> fuzzrOfT2,
        FuzzrOf<T3> fuzzrOfT3,
        FuzzrOf<T4> fuzzrOfT4,
        FuzzrOf<T5> fuzzrOfT5) =>
            from t1 in fuzzrOfT1
            from t2 in fuzzrOfT2
            from t3 in fuzzrOfT3
            from t4 in fuzzrOfT4
            from t5 in fuzzrOfT5
            select (t1, t2, t3, t4, t5);

    /// <summary>
    /// Creates a Fuzzr that combines six source Fuzzrs into a tuple of their generated values.
    /// Use for fixed-size heterogeneous input generation where six independently generated values should be produced together.
    /// </summary>
    public static FuzzrOf<(T1, T2, T3, T4, T5, T6)> Tuple<T1, T2, T3, T4, T5, T6>(
        FuzzrOf<T1> fuzzrOfT1,
        FuzzrOf<T2> fuzzrOfT2,
        FuzzrOf<T3> fuzzrOfT3,
        FuzzrOf<T4> fuzzrOfT4,
        FuzzrOf<T5> fuzzrOfT5,
        FuzzrOf<T6> fuzzrOfT6) =>
            from t1 in fuzzrOfT1
            from t2 in fuzzrOfT2
            from t3 in fuzzrOfT3
            from t4 in fuzzrOfT4
            from t5 in fuzzrOfT5
            from t6 in fuzzrOfT6
            select (t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Creates a Fuzzr that combines seven source Fuzzrs into a tuple of their generated values.
    /// Use for fixed-size heterogeneous input generation where seven independently generated values should be produced together.
    /// </summary>
    public static FuzzrOf<(T1, T2, T3, T4, T5, T6, T7)> Tuple<T1, T2, T3, T4, T5, T6, T7>(
        FuzzrOf<T1> fuzzrOfT1,
        FuzzrOf<T2> fuzzrOfT2,
        FuzzrOf<T3> fuzzrOfT3,
        FuzzrOf<T4> fuzzrOfT4,
        FuzzrOf<T5> fuzzrOfT5,
        FuzzrOf<T6> fuzzrOfT6,
        FuzzrOf<T7> fuzzrOfT7) =>
            from t1 in fuzzrOfT1
            from t2 in fuzzrOfT2
            from t3 in fuzzrOfT3
            from t4 in fuzzrOfT4
            from t5 in fuzzrOfT5
            from t6 in fuzzrOfT6
            from t7 in fuzzrOfT7
            select (t1, t2, t3, t4, t5, t6, t7);
}