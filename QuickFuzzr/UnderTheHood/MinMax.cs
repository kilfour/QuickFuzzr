namespace QuickFuzzr.UnderTheHood;

public static class MinMax
{
    public static FuzzrOf<T> Check<T>(T min, T max, FuzzrOf<T> fuzzr) where T : IComparable<T>
    {
        if (min.CompareTo(max) > 0)
            throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
        return fuzzr;
    }
    //	=> MinMax.Check(min, max, ---- );
}