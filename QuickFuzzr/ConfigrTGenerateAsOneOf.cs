using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static FuzzrOf<Intent> AsOneOf(params Type[] types)
    {
        return
            s =>
                {
                    s.InheritanceInfo[typeof(T)] = [.. types];
                    return new Result<Intent>(Intent.Fixed, s);
                };
    }
}