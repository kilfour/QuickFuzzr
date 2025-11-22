namespace QuickFuzzr.Evil;

public static partial class Evilr
{
    public static FuzzrOf<string> String()
    {
        var evilChars = new[] { '\0', /* … */ };
        var evilCharFuzzr = Fuzzr.OneOf(evilChars);

        var evilStringFuzzr =
            Fuzzr.OneOf(
                Fuzzr.String(evilCharFuzzr),
                Fuzzr.Constant(string.Empty),
                Fuzzr.Constant("\t"),
                Fuzzr.Constant("\r\n")
            );

        return evilStringFuzzr;
    }
}

public static partial class Evilizr
{
    public static FuzzrOf<Intent> Strings()
    {
        return Configr.Primitive(Evilr.String());
    }
}