namespace QuickFuzzr.Tests;

public static class Evilizr
{
    public static FuzzrOf<Intent> Strings()
    {
        var evilChars = new[] { '\0' };
        var evilCharFuzzr = Fuzzr.OneOf(evilChars);
        return Configr.Primitive(Fuzzr.OneOf(Fuzzr.String(evilCharFuzzr), Fuzzr.String(null!)));
    }
}