using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    public static FuzzrOf<Intent> Combine(
        params FuzzrOf<Intent>[] configrs)
    {
        ArgumentNullException.ThrowIfNull(configrs);

        return state =>
        {
            foreach (var configr in configrs)
            {
                ArgumentNullException.ThrowIfNull(configr);
                configr(state);
            }

            return Result.Unit(state);
        };
    }
}