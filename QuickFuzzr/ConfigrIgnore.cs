using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    /// <summary>
    /// Creates a fuzzr that configures property exclusion rules using a predicate to determine which properties to ignore during generation.
    /// Use for skipping properties that should not be auto-generated, such as computed fields, navigation properties, or sensitive data.
    /// </summary>
    public static FuzzrOf<Intent> Ignore(Func<PropertyInfo, bool> predicate) =>
        state =>
        {
            state.GeneralStuffToIgnore.Add(predicate);
            return Result.Unit(state);
        };
}