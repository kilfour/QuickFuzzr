namespace QuickFuzzr.UnderTheHood;

public static class MinMax
{
    public static void Check<T>(T min, T max) where T : IComparable<T>
    {
        if (min.CompareTo(max) > 0)
            throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
    }

    public static FuzzrOf<T> Check<T>(T min, T max, FuzzrOf<T> fuzzr) where T : IComparable<T>
    {
        if (min.CompareTo(max) > 0)
            throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
        return fuzzr; //remove use Chain.It
    }
    //	=> MinMax.Check(min, max, ---- );
}