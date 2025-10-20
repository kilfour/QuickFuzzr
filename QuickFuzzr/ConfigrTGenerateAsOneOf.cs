using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static Generator<Unit> AsOneOf(params Type[] types)
    {
        return
            s =>
                {
                    s.InheritanceInfo[typeof(T)] = types.ToList();
                    return new Result<Unit>(Unit.Instance, s);
                };
    }
}